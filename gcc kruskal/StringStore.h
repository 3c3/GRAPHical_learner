#ifndef STRING_STORE
#define STRING_STORE

#include <cstring>

class StringStore
{
private:
	char** szBuffer;
	unsigned* hashBuffer;
	int idx = 0;

	const static int hashBase = 117;
	const static int hashMod = 1737313;

	unsigned GetHash(char* str)
	{
		int len = strlen(str);
		return GetHash(str, len);
	}

	unsigned GetHash(char* str, int len)
	{
		int h = 0;
		for (int i = 0; i < len; i++)
		{
			h *= hashBase;
			h += str[i];
			h %= hashMod;
		}
		return h;
	}

	int FindString(char* sz, int len)
	{
		unsigned hash = GetHash(sz, len);
		for (int i = 0; i < idx; i++)
		{
			if (hash == hashBuffer[i]) return i;
		}
		return -1;
	}

public:
	StringStore()
	{
		szBuffer = new char*[16384];
		hashBuffer = new unsigned[16384];
	}

	char* Store(char* sz)
	{
		int len = strlen(sz);

		int pos = FindString(sz, len);
		if (pos != -1 && strncmp(sz, szBuffer[pos], len) == 0) return szBuffer[pos];

		szBuffer[idx] = new char[len + 1];
		memcpy(szBuffer[idx], sz, len + 1);
		char* result = szBuffer[idx];

		hashBuffer[idx] = GetHash(sz, len);
		idx++;
		return result;
	}
};

#endif