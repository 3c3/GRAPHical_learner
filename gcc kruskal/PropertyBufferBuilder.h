#ifndef PROPERTY_BUIDLER
#define PPROPETY_BUILDER

#include "Property.h"

#include <cstring>
#include <iostream>

using namespace std;

struct PropertyHeader
{
	char type;
	char id;
	char* name;
};

class PropertyBuffer
{
private:
	int buffCapacity;
	char* buffer;

	int* pBufferSize;
	char* pNumberOfProperties;

	char propertyCounter = 0;

	int bufferPosition = 5;

	void resize()
	{
		char* oldBuffer = buffer;
		buffer = new char[buffCapacity * 2];
		memcpy(buffer, oldBuffer, buffCapacity);
		buffCapacity *= 2;
		delete[] oldBuffer;

		pBufferSize = (int*)buffer;
		pNumberOfProperties = (char*)(buffer + 5);
	}

	int GetRemainingSpace()
	{
		return buffCapacity - bufferPosition;
	}

public:

	PropertyBuffer()
	{
		buffer = new char[1024];
		buffCapacity = 1024;

		pBufferSize = (int*)buffer;
		pNumberOfProperties = (char*)(buffer + 5);
	}

	char AddBoolProperty(char propertyType, char* szName, bool defaultValue)
	{
		if ((propertyType != TYPE_BOOL) && (propertyType != TYPE_VISIBLE) && (propertyType != TYPE_MARKED)) return -1;

		if (GetRemainingSpace() < (sizeof(PropertyHeader)+1)) resize();

		buffer[bufferPosition] = propertyType;
		bufferPosition++;
		buffer[bufferPosition] = propertyCounter;
		bufferPosition++;
		char* szBuff = (char*)(buffer + bufferPosition);
		szBuff = szName;
		bufferPosition += 4;


		buffer[bufferPosition] = (char)defaultValue;
		bufferPosition++;

		*pBufferSize = bufferPosition;

		propertyCounter++;

		*pNumberOfProperties = propertyCounter;
		cout << (*pBufferSize) << " ; " << (int)(*pNumberOfProperties) << endl;

		return propertyCounter - 1;
	}

	char* GetBuffer()
	{
		return buffer;
	}
};

#endif