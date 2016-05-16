#ifndef PROPERTY_H
#define PROPERTY_H

#define VERTEX_PROPERTY 0
#define EDGE_PROPERTY 1<<7

#define TYPE_BOOL 0
#define TYPE_MARKED 1
#define TYPE_VISIBLE 2
#define TYPE_INT 3
#define TYPE_LONG 4
#define TYPE_FLOAT 5
#define TYPE_DOUBLE 6
#define TYPE_RGB24 7
#define TYPE_STRING 8
#define TYPE_COST 1<<6

#include <iostream>
using namespace std;

struct ColorRGB
{
	char r, g, b;
	ColorRGB()
	{
		r = 0;
		g = 0;
		b = 0;
	}
	ColorRGB(char red, char green, char blue)
	{
		r = red;
		b = blue;
		g = green;
	}
};

struct PropertyBuffer
{
	void* primary;
	void* secondary;
};
#endif