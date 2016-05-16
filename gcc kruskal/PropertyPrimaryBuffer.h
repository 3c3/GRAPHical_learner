#ifndef PROPERTY_BUFFER_PRIMARY
#define PROPERTY_BUFFER_PRIMARY

#include "Property.h"

class PropertyPrimaryBuffer
{
private:
	char* buffer;
	int* pLen;
	int* n;

	char* position;

public:
	PropertyPrimaryBuffer()
	{
		buffer = new char[16384];
		pLen = (int*)buffer;
		n = (int*)(buffer + 4);
		*n = 0;
		position = buffer + 8;

		*pLen = 8;
	}

	~PropertyPrimaryBuffer()
	{
		delete[] buffer;
	}


	void Write(char id, bool value)
	{
		*position = id;
		position++;
		*position = (char)value;
		position++;

		*pLen+=2;
		(*n)++;
	}

	void Write(char id, int value)
	{
		*position = id;
		position++;

		int* pv = (int*)position;
		*pv = value;
		position += 4;

		*pLen += 5;
		(*n)++;
	}

	void Write(char id, long long value)
	{
		*position = id;
		position++;

		long long* pv = (long long*)position;
		*pv = value;
		position += 8;

		*pLen += 9;
		(*n)++;
	}

	void Write(char id, float value)
	{
		*position = id;
		position++;

		float* pv = (float*)position;
		*pv = value;
		position += 4;

		*pLen += 5;
		(*n)++;
	}

	void Write(char id, double value)
	{
		*position = id;
		position++;

		double* pv = (double*)position;
		*pv = value;
		position += 8;

		*pLen += 9;
		(*n)++;
	}

	void Write(char id, ColorRGB value)
	{
		*position = id;
		position++;

		ColorRGB* pv = (ColorRGB*)position;
		*pv = value;
		position += 3;

		*pLen += 4;
		(*n)++;
	}

	void Write(char id, char* value)
	{
		*position = id;
		position++;

		char** pv = (char**)position;
		*pv = value;
		position += 4;

		*pLen += 5;
		(*n)++;
	}

	char* GetBuffer()
	{
		//cout << "Primary buffer: ";
		//Dump();
		return buffer;
	}

	void Dump()
	{
		unsigned char* c = (unsigned char*)buffer;
		int len = *pLen;
		for (int i = 0; i < len; i++) cout << (unsigned)c[i] << " ";
		cout << endl;
	}
};

#endif