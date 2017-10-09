using System;
using System.Collections.Generic;

public struct KBVector2
{
	public float X { get { return _x; } set { _x = value; } }
	public float Y { get { return _y; } set { _y = value; } }

	public KBVector2(float x, float y)
	{
		_x = x;
		_y = y;
	}

	public float _x;
	public float _y;
}
