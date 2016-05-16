#ifndef DEBUG_H
#define DEBUG_H

#include <iostream>

using namespace std;

void DumpBuffer(void* buff, int len)
{
	unsigned char* c = (unsigned char*)buff;
	for (int i = 0; i < len; i++) cout << (unsigned)c << " ";
	cout << endl;
}

#endif