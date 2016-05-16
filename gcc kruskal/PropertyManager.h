#ifndef PROPERTYMAN
#define PROPERTYMAN

#include "ChangedPropertyBuffer.h"
#include "PropertyPrimaryBuffer.h"
#include "PropertySecondaryBuffer.h"
#include "StringStore.h"

#define dataMask 0x3f
#define specMask 0xc0

class PropertyManager
{
private:
	char propertyType[256];
	char propertyCounter = 0;

	PropertyPrimaryBuffer* ppb;
	PropertySecondaryBuffer* psb;
	StringStore* ss;

	PropertyBuffer* pb;
	ChangedPropertyBuffer* vertexBuffer, *edgeBuffer;

public:
	PropertyManager()
	{
		ss = new StringStore();
		vertexBuffer = new ChangedPropertyBuffer();
		edgeBuffer = new ChangedPropertyBuffer();
	}

	void BeginRegistration();
	PropertyBuffer* MakePropertyBuffer();
	void EndRegistration();

	char RegisterProperty(char type, char* name, bool value);
	char RegisterProperty(char type, char* name, int value);
	char RegisterProperty(char type, char* name, long long value);
	char RegisterProperty(char type, char* name, float value);
	char RegisterProperty(char type, char* name, double value);
	char RegisterProperty(char type, char* name, ColorRGB value);
	char RegisterProperty(char type, char* name, char* value);

	char* GetVertexBuffer()
	{
		return vertexBuffer->GetBuffer();
	}
	char* GetEdgeBuffer()
	{
		return edgeBuffer->GetBuffer();
	}
	void EndChanges();

	bool SetProperty(int elementId, char propertyId, bool value);
	bool SetProperty(int elementId, char propertyId, int value);
	bool SetProperty(int elementId, char propertyId, float value);
	bool SetProperty(int elementId, char propertyId, double value);
	bool SetProperty(int elementId, char propertyId, ColorRGB value);
	bool SetProperty(int elementId, char propertyId, char* value);
};

void PropertyManager::BeginRegistration()
{
	psb = new PropertySecondaryBuffer();
	ppb = new PropertyPrimaryBuffer();
}

void PropertyManager::EndRegistration()
{
	delete psb;
	delete ppb;
}

PropertyBuffer* PropertyManager::MakePropertyBuffer()
{
	pb = new PropertyBuffer;
	pb->primary = ppb->GetBuffer();
	pb->secondary = psb->GetBuffer();


	/*cout << "in propertyman: \n";
	unsigned char* c = (unsigned char*)((char*)(pb->secondary) + 5);
	for (int i = 0; i < 4; i++) cout << (unsigned)c[i] << " ";
	cout << endl;

	char* sz = *((char**)c);
	cout << sz << endl;
	*/
	return pb;
}

char PropertyManager::RegisterProperty(char type, char* name, bool value)
{
	char valueType = type & dataMask;

	if (valueType < TYPE_BOOL && valueType > TYPE_VISIBLE)
	{
		type &= specMask;
		type |= TYPE_BOOL;
	}

	char* storedName = ss->Store(name);
	psb->Write(type, storedName);
	ppb->Write(propertyCounter, value);

	propertyType[propertyCounter] = type;
	propertyCounter++;
	return propertyCounter - 1;
}

char PropertyManager::RegisterProperty(char type, char* name, int value)
{
	char rtype = type & dataMask;
	if (rtype == TYPE_LONG)
	{
		return RegisterProperty(type & (specMask) | TYPE_LONG, name, (long long)value);
	}
	char* storedName = ss->Store(name);

	type = type&(specMask) | TYPE_INT;
	psb->Write(type, storedName);
	ppb->Write(propertyCounter, value);

	propertyType[propertyCounter] = type;
	propertyCounter++;
	return propertyCounter - 1;
}

char PropertyManager::RegisterProperty(char type, char* name, long long value)
{
	char* storedName = ss->Store(name);

	type = type&specMask | TYPE_LONG;
	psb->Write(type, storedName);
	ppb->Write(propertyCounter, value);

	propertyType[propertyCounter] = type;
	propertyCounter++;
	return propertyCounter - 1;
}

char PropertyManager::RegisterProperty(char type, char* name, float value)
{
	char* storedName = ss->Store(name);

	type = type&specMask | TYPE_FLOAT;
	psb->Write(type, storedName);
	ppb->Write(propertyCounter, value);

	propertyType[propertyCounter] = type;
	propertyCounter++;
	return propertyCounter - 1;
}

char PropertyManager::RegisterProperty(char type, char* name, double value)
{
	char* storedName = ss->Store(name);

	type = type&specMask | TYPE_DOUBLE;
	psb->Write(type, storedName);
	ppb->Write(propertyCounter, value);

	propertyType[propertyCounter] = type;
	propertyCounter++;
	return propertyCounter - 1;
}

char PropertyManager::RegisterProperty(char type, char* name, ColorRGB value)
{
	char* storedName = ss->Store(name);

	type = type&specMask | TYPE_RGB24;
	psb->Write(type, storedName);
	ppb->Write(propertyCounter, value);

	propertyType[propertyCounter] = type;
	propertyCounter++;
	return propertyCounter - 1;
}

char PropertyManager::RegisterProperty(char type, char* name, char* value)
{
	char* storedName = ss->Store(name);

	type = type&specMask | TYPE_STRING;
	psb->Write(type, storedName);

	char* storedVal = ss->Store(value);

	ppb->Write(propertyCounter, storedVal);

	propertyType[propertyCounter] = type;
	propertyCounter++;
	return propertyCounter - 1;
}

bool PropertyManager::SetProperty(int elementId, char propertyId, bool value)
{
	char type = propertyType[propertyId];
	char rtype = type & dataMask;

	if (rtype > TYPE_VISIBLE) return false;

	if (type & 0x80) // ребро
	{
		edgeBuffer->Write(elementId, propertyId, value);
	}
	else vertexBuffer->Write(elementId, propertyId, value);

	return true;
}

bool PropertyManager::SetProperty(int elementId, char propertyId, int value)
{
	char type = propertyType[propertyId];
	char rtype = type & dataMask;

	if (rtype != TYPE_INT) return false;

	if (type & 0x80) // ребро
	{
		edgeBuffer->Write(elementId, propertyId, value);
	}
	else vertexBuffer->Write(elementId, propertyId, value);
	//vertexBuffer->Dump();
	return true;
}

bool PropertyManager::SetProperty(int elementId, char propertyId, float value)
{
	char type = propertyType[propertyId];
	char rtype = type & dataMask;

	if (rtype != TYPE_FLOAT) return false;

	if (type & 0x80) // ребро
	{
		edgeBuffer->Write(elementId, propertyId, value);
	}
	else vertexBuffer->Write(elementId, propertyId, value);
	return true;
}

bool PropertyManager::SetProperty(int elementId, char propertyId, double value)
{
	char type = propertyType[propertyId];
	char rtype = type & dataMask;

	if (rtype != TYPE_DOUBLE) return false;

	if (type & 0x80) // ребро
	{
		edgeBuffer->Write(elementId, propertyId, value);
	}
	else vertexBuffer->Write(elementId, propertyId, value);
	return true;
}

bool PropertyManager::SetProperty(int elementId, char propertyId, ColorRGB value)
{
	char type = propertyType[propertyId];
	char rtype = type & dataMask;

	if (rtype != TYPE_RGB24) return false;

	if (type & 0x80) // ребро
	{
		edgeBuffer->Write(elementId, propertyId, value);
	}
	else vertexBuffer->Write(elementId, propertyId, value);
	return true;
}

bool PropertyManager::SetProperty(int elementId, char propertyId, char* value)
{
	char type = propertyType[propertyId];
	char rtype = type & dataMask;

	if (rtype != TYPE_STRING) return false;

	char* storedValue = ss->Store(value);

	if (type & 0x80) // ребро
	{
		edgeBuffer->Write(elementId, propertyId, storedValue);
	}
	else vertexBuffer->Write(elementId, propertyId, storedValue);
	return true;
}

void PropertyManager::EndChanges()
{
	edgeBuffer->Flush();
	vertexBuffer->Flush();
}

#endif