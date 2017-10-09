using System;
using System.Collections.Generic;

//左手坐标系
//
//  y |  / z
//    | /
//    |/_________ x
//
public struct KBVector3
{
    public static KBVector3 ONE { get { return new KBVector3(1.0f, 1.0f, 1.0f); } }
    public static KBVector3 ZERO { get { return new KBVector3(0.0f, 0.0f, 0.0f); } }
    public static KBVector3 UP { get { return new KBVector3(0.0f, 1.0f, 0.0f); } }
    public static KBVector3 FORWARD { get { return new KBVector3(0.0f, 0.0f, 1.0f); } }
    public static KBVector3 LEFT { get { return new KBVector3(1.0f, 0.0f, 0.0f); } }

    public float X { get { return _x; } set { _x = value; } }
    public float Y { get { return _y; } set { _y = value; } }
    public float Z { get { return _z; } set { _z = value; } }

    public KBVector3(float x, float y, float z)
    {
        _x = x;
        _y = y;
        _z = z;
    }

    public KBVector3(KBVector3 sourceVector)
    {
        _x = sourceVector.X;
        _y = sourceVector.Y;
        _z = sourceVector.Z;
    }

    public float Magnitude()
    {
        return KBMathDefine.Sqrt(X * X + Y * Y + Z * Z);
    }

    public float LengthSquared()
    {
        return X * X + Y * Y + Z * Z;
    }

    public KBVector3 Normalize()
    {
        return this / this.Magnitude();
    }

    // Dot(A ,B) = valueA * valueB * cos(Angle) 几何意义（投影）
    public static float Dot(KBVector3 v01, KBVector3 v02)
    {
        return v01.X * v02.X + v01.Y * v02.Y + v01.Z * v02.Z;
    }

    // value(A * B) = valueA * valueB * sin(Angle) 平行四边形的面积 （行列式的值）
    public static KBVector3 Cross(KBVector3 v01, KBVector3 v02)
    {
        float x = v01.Y * v02.Z - v01.Z * v02.Y;
        float y = v01.Z * v02.X - v01.X * v02.Z;
        float z = v01.X * v02.Y - v01.Y * v02.X;
        return new KBVector3(x, y, z);
    }

    public static KBVector3 operator +(KBVector3 a, KBVector3 b)
    {
        return new KBVector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    }

    public static KBVector3 operator -(KBVector3 a, KBVector3 b)
    {
        return new KBVector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    }

    public static KBVector3 operator -(KBVector3 a)
    {
        return new KBVector3(-a.X, -a.Y, -a.Z);
    }

    public static KBVector3 operator *(KBVector3 a, float d)
    {
        return new KBVector3(a.X * d, a.Y * d, a.Z * d);
    }

    public static KBVector3 operator *(float d, KBVector3 a)
    {
        return new KBVector3(a.X * d, a.Y * d, a.Z * d);
    }

    public static KBVector3 operator /(KBVector3 a, float d)
    {
        return new KBVector3(a.X / d, a.Y / d, a.Z / d);
    }

    public static KBVector3 Lerp(KBVector3 from, KBVector3 to, float value)
    {
        float x = KBMathDefine.Lerp(from.X, to.X, value);
        float y = KBMathDefine.Lerp(from.Y, to.Y, value);
        float z = KBMathDefine.Lerp(from.Z, to.Z, value);
        return new KBVector3(x, y, z);
    }

    public static KBVector3 Normalize(KBVector3 vector)
    {
        return vector / vector.Magnitude();
    }

    public static float Distance(KBVector3 pos1, KBVector3 pos2)
    {
        KBVector3 pos = pos1 - pos2;
        return pos.Magnitude();
    }

    float _x;
    float _y;
    float _z;
}
