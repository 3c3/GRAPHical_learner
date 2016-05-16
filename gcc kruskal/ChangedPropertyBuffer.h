#ifndef CP_BUFFER_H
#define CP_BUFFER_H

#include "Property.h"

class ChangedPropertyBuffer
{
private:
	char* buffer;
	int* position;
	char* current;

public:
	ChangedPropertyBuffer()
	{
		buffer = new char[16384];
		position = (int*)buffer;
		*position = 0;
		current = buffer + 4;
	}

	template<typename T>
	void Write(int elementId, char propertyId, T value)
	{
		int* pi = (int*)current;
		*pi = elementId;
		//cout << "elementId: " << elementId << "; written int " << *pi << endl;
		current += 4;
		*current = propertyId;
		current++;

		int sizeT = sizeof(T);
		//cout << "sizeT: " << sizeT << endl;
		T* valPtr = (T*)current;
		*valPtr = value;
		current += sizeT;
		(*position) += 5 + sizeT;

		//Dump();
	}


	char* GetBuffer()
	{
		return buffer;
	}

	void Flush()
	{
		(*position) = 0;
		current = buffer + 4;
	}

	void Dump()
	{
		unsigned char* c = (unsigned char*)(buffer + 4);
		for (int i = 0; i < *(position); i++) cout << (int)c[i] << " ";
		cout << endl;
	}
};

#endif