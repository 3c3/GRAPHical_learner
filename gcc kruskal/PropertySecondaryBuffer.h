#ifndef PROPERTY_BUFFER_SECONDARY
#define PROPERTY_BUFFER_SECONDARY

#include "Property.h"

#include <cstring>

class PropertySecondaryBuffer
{
private:
	char* buffer;
	int* n;
	char* position;

public:
	PropertySecondaryBuffer()
	{
		buffer = new char[2048];
		n = (int*)buffer;
		*n = 0;
		position = buffer + 4;
	}

	~PropertySecondaryBuffer()
	{
		delete[] buffer;
	}

	void Write(char type, char* szName)
	{
		(*position) = type;
		position++;
		
		char** val = (char**)position;
		*val = szName;

		/*unsigned char* c = (unsigned char*)position;
		for (int i = 0; i < 4; i++) cout << (unsigned)c[i] << " ";
		cout << endl;*/

		position += 4;
		(*n)++;
	}

	char* GetBuffer()
	{
		/*cout << "Secondary buffer: ";
		Dump();*/
		return buffer;
	}

	void Dump()
	{
		unsigned char* c = (unsigned char*)buffer;
		int len = (*n) * 5 + 4;
		for (int i = 0; i < len; i++) cout << (unsigned)c[i] << " ";
		cout << endl;
	}
};

#endif