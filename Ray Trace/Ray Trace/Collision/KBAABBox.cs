using System;
using System.Collections.Generic;

/// <summary>
/// 轴对齐包围合
/// </summary>
public class KBAABBox
{
	public KBVector3 MinPos { get { return _minPos; } }
	public KBVector3 MaxPos { get { return _maxPos; } }
	public KBVector3 Center { get { return (_maxPos + _minPos) / 2; } }

	public KBVector3 Size
	{
		get
		{
			float x = KBMathDefine.Abs(_maxPos.X - _minPos.X);
			float y = KBMathDefine.Abs(_maxPos.Y - _minPos.Y);
			float z = KBMathDefine.Abs(_maxPos.Z - _minPos.Z);
			return new KBVector3(x, y, z);
		}
	}

	public KBAABBox()
	{
		_minPos.X = Int32.MaxValue * 1.0f;
		_minPos.Y = Int32.MaxValue * 1.0f;
		_minPos.Z = Int32.MaxValue * 1.0f;
		//
		_maxPos.X = Int32.MinValue * 1.0f;
		_maxPos.Y = Int32.MinValue * 1.0f;
		_maxPos.Z = Int32.MinValue * 1.0f;
	}

    public KBAABBox(KBVector3 minPoint, KBVector3 maxPoint)
    {
        _minPos.X = minPoint.X;
        _minPos.Y = minPoint.Y;
        _minPos.Z = minPoint.Z;
        //
        _maxPos.X = maxPoint.X;
        _maxPos.Y = maxPoint.Y;
        _maxPos.Z = maxPoint.Z;
    }

    public void AddPos(KBVector3 pos)
	{
		if (pos.X < _minPos.X) _minPos.X = pos.X;
		if (pos.X > _maxPos.X) _maxPos.X = pos.X;
		if (pos.Y < _minPos.Y) _minPos.Y = pos.Y;
		if (pos.Y > _maxPos.Y) _maxPos.Y = pos.Y;
		if (pos.Z < _minPos.Z) _minPos.Z = pos.Z;
		if (pos.Z > _maxPos.Z) _maxPos.Z = pos.Z;
	}

	public bool IsIntersects(KBAABBox boundBox)
	{
		if (MinPos.X <= boundBox.MaxPos.X
			&& MinPos.Y <= boundBox.MaxPos.Y
			&& MinPos.Z <= boundBox.MaxPos.Z
			&& MaxPos.X >= boundBox.MinPos.X
			&& MaxPos.Y >= boundBox.MinPos.Y
			&& MaxPos.Z >= boundBox.MaxPos.Z)
		{
			return true;
		}
		//
		return false;
	}

	public bool IsIntersects(KBVector3 vec)
	{
		return true;
	}

	public static bool IsIntersects(KBAABBox boundBox01, KBAABBox boundBox02)
	{
		if (boundBox01.MinPos.X <= boundBox02.MaxPos.X
			&& boundBox01.MinPos.Y <= boundBox02.MaxPos.Y
			&& boundBox01.MinPos.Z <= boundBox02.MaxPos.Z
			&& boundBox01.MaxPos.X >= boundBox02.MinPos.X
			&& boundBox01.MaxPos.Y >= boundBox02.MinPos.Y
			&& boundBox01.MaxPos.Z >= boundBox02.MaxPos.Z)
		{
			return true;
		}
		//
		return false;
	}

	KBVector3 _minPos;
	KBVector3 _maxPos;
}
