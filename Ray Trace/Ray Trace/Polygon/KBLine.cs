using System;
using System.Collections.Generic;

public class KBLine
{
	public KBVector3 StartPos { get { return _startPois; } }
	public KBVector3 EndPos { get { return _endPos; } }

	public KBLine(KBVector3 startPos, KBVector3 endPos)
	{
		_startPois = startPos;
		_endPos = endPos;
	}

	public bool IsIntersects(KBLine line)
	{
		return true;
	}

	public static bool IsIntersects(KBLine line01, KBLine line02)
	{
		return true;
	}

	KBVector3 _startPois;
	KBVector3 _endPos;
}
