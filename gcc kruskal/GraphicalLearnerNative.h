#ifndef GRAPHICAL_LEARNER_NATIVE_H
#define GRAPHICAL_LEARNER_NATIVE_H

#include <Windows.h>
#include <sstream>
#include <string>
#include "PropertyManager.h"

using namespace std;

// Структурата с необходимите данни при стартиране
struct StartUpData
{
	int threadId; // id на нишката на алгоритъма
	void* pGraphData; // сочи към буфера с данните за графа
	void* pPropertyData; // буфера с данните за свойствата
	volatile void* pVertexBuffer; // буфер с променени свойства на върхове
	volatile void* pEdgeBuffer; // буфер с променени свойства на ребра
	volatile bool* pControlByte; // показва дали програмата е паузирана
	void* fPtr; // функция за намиране дължина на C string
};

StartUpData sud;

// Структура за връх
struct GL_Edge
{
	int src, dest; // източник и дестинация(върхове)
};

// Намира дължината на C string. Извиква се от C# 
// sz - C string(sz = string zero terminated)
int GetLen(char* sz)
{
	return strlen(sz);
}

///////////////////////////////////////////////
// Данни за графа - върхове, ребра, насоченост
char graphData[1000000]; // буфера, съхраняващ данните

// Формат: [брой върхове - 4 байта] [брой ребра - 4 байта] [насоченост - 1 байт] [списък с ребра - 8 байта всяко] 
int* nVert = (int*)graphData; // броя върхове
int* nEdge = (int*)(graphData + 4); // броя ребра
bool* directed = (bool*)(graphData + 8); // дали графа е насочен
GL_Edge* gl_edges = (GL_Edge*)(graphData + 9); // ребрата на графа

int currentEdge = 0;

// Създава ребро във визуалния граф
void AddEdge(int src, int dest)
{
	GL_Edge* e = gl_edges + currentEdge;
	e->src = src;
	e->dest = dest;
	currentEdge++;
}

void SetVertices(int n)
{
	*nVert = n;
}

void SetEdges(int n)
{
	*nEdge = n;
}

void SetDirected(bool value)
{
	*directed = value;
}

///////////////////////////////////////////////

///////////////////////////////////////////////
// Windows-ски данни за стартиране на процес и управление на нишки 
HANDLE hProcess; // handle към текущия процес
HANDLE hThread; // handle към текущата нишка

// Адрес на C# програмата.
char programLoc[1024];
///////////////////////////////////////////////

// флаг, показващ дали алгоритъма(тази програма) е спряна
volatile bool isSuspended = false;

// управлява свойствата
PropertyManager pm;

// Стартира визуалната среда. Свойствата трябва да бъдат регистрирани преди извикването на тази функция
void StartGui()
{
	int pid = GetCurrentProcessId();
	// попълване на стартовата структура
	sud.pGraphData = (void*)graphData;
	sud.threadId = GetCurrentThreadId();
	sud.pControlByte = &isSuspended;
	sud.pPropertyData = (void*)pm.MakePropertyBuffer();
	sud.fPtr = (void*)GetLen;
	sud.pVertexBuffer = (void*)pm.GetVertexBuffer();
	sud.pEdgeBuffer = (void*)pm.GetEdgeBuffer();

	// първите данни се обменят чрез командния ред
	// формат: [id на процес] [адрес на стартова структура]
	stringstream ss;
	ss << pid << ";" << (&sud);

	char commBuff[1024];
	ss >> commBuff;

	// Windows-ска процедура за пускане на програма
	STARTUPINFOA si;
	PROCESS_INFORMATION pi;

	ZeroMemory(&si, sizeof(si));
	ZeroMemory(&pi, sizeof(pi));
	si.cb = sizeof(si);

	bool ok = CreateProcessA(programLoc, commBuff,
		NULL, NULL,
		false, CREATE_NEW_CONSOLE,
		NULL, NULL,
		&si, &pi);

	if (ok)
	{
		cout << "Process launched!\n";
		SuspendThread(GetCurrentThread());
	}
	else
	{
		cout << "fail: " << GetLastError() << endl;
		cout << programLoc << endl;
	}
}

// Спира(паузира) алгоритъма. 
void Pause()
{
	isSuspended = true;
	SuspendThread(GetCurrentThread());
	isSuspended = false;

	pm.EndChanges();
}

// Функции за работа с регистри
DWORD(__stdcall *RegOpenKeyExA_)(HKEY hKey, LPCSTR lpSubKey, DWORD ulOptions, REGSAM samDesired, PHKEY phkResult);
DWORD(__stdcall *RegQueryValueExA_)(HKEY, LPCSTR, LPCSTR, LPDWORD, LPBYTE, LPDWORD);

// Прави първоначални настройки
bool Initialise()
{
	// зарежда се необходимата библиотека динамично
	HMODULE hAdvApi = LoadLibraryA("Advapi32.dll");

	if (hAdvApi == 0)
	{
		cout << "Couldn't load Advapi32.dll!\n";
		return false;
	}

	RegOpenKeyExA_ = (DWORD(__stdcall*)(HKEY hKey, LPCSTR lpSubKey, DWORD ulOptions, REGSAM samDesired, PHKEY phkResult))GetProcAddress(hAdvApi, "RegOpenKeyExA");
	RegQueryValueExA_ = (DWORD(__stdcall*)(HKEY, LPCSTR, LPCSTR, LPDWORD, LPBYTE, LPDWORD))GetProcAddress(hAdvApi, "RegQueryValueExA");

	HKEY hKey;
	LONG result = RegOpenKeyExA_(HKEY_CURRENT_USER, "Software\\KNN\\Graphical Learner", 0, KEY_QUERY_VALUE, &hKey);

	if (result != ERROR_SUCCESS)
	{
		cout << "Couldn't open key! This could mean GRAPHical Learner is not installed.\n";
		return false;
	}

	DWORD dwType = REG_SZ;
	DWORD buffSize = 1024;

	result = RegQueryValueExA_(hKey, "InstallPath", 0, &dwType, (LPBYTE)programLoc, &buffSize);

	if (result != ERROR_SUCCESS)
	{
		cout << "Couldn't read key! Make sure program is installed.\n";
		return false;
	}

	//cout << programLoc << endl;
	strcat(programLoc, "\\NativeAlgo\\bin\\Release\\NativeAlgo.exe");
	//cout << programLoc << endl;

	pm.BeginRegistration();
	return true;
}
#endif