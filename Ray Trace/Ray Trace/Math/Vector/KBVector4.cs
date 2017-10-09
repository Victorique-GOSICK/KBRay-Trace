using System;
using System.Collections.Generic;

public struct KBVector4
{
	public float X { get { return _x; } set { _x = value; } }
	public float Y { get { return _y; } set { _y = value; } }
	public float Z { get { return _z; } set { _z = value; } }
	public float W { get { return _w; } set { _w = value; } }

	public KBVector4(float x, float y, float z, float w)
	{
		_x = x;
		_y = y;
		_z = z;
		_w = w;
	}

	float _x;
	float _y;
	float _z;
	float _w;
}
