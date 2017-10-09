using System;
using System.Collections.Generic;

public class KBPlane
{
    public KBVector3 Normal;//平面法向量
    public float D;//平面原点的距离

    public KBPlane(float x, float y, float z, float d)
    {
        Normal = new KBVector3(x, y, z);
        this.D = d;
    }

    public KBPlane(KBVector3 normal, float d)
    {
        this.Normal = normal;
        this.D = d;
    }

    public KBPlane(KBVector4 value)
    {
        Normal = new KBVector3(value.X, value.Y, value.Z);
        D = value.W;
    }

    public static KBPlane CreateFromVertices(KBVector3 point1, KBVector3 point2, KBVector3 point3)
    {
        KBVector3 a = point2 - point1;
        KBVector3 b = point3 - point1;
        KBVector3 n = KBVector3.Cross(a, b);
        KBVector3 normal = KBVector3.Normalize(n);
        float d = -KBVector3.Dot(normal, point1); //ax + by + cz + d =0 -> d = -(a,b,c)(x,y,z)
        return new KBPlane(normal, d);
    }

    public static KBPlane Normalize(KBPlane value)
    {
        const float FLT_EPSILON = 1.192092896e-07f;
        float normalLengthSquared = value.Normal.LengthSquared();
        if (KBMathDefine.Abs(normalLengthSquared - 1.0f) < FLT_EPSILON)
        {
            return value;
        }
        //
        float normalLength = KBMathDefine.Sqrt(normalLengthSquared);
        return new KBPlane(value.Normal / normalLength, value.D / normalLength);
    }

    /// <summary>
    /// 法线自身坐标转为世界坐标的话要用objectToWorld逆矩阵转置
    /// </summary>
    /// <returns></returns>
    public static KBPlane Transform(KBPlane plane, KBMatrix4x4 matrix)
    {
        KBMatrix4x4 m;
        KBMatrix4x4.Invert(matrix, out m);
        float x = plane.Normal.X, y = plane.Normal.Y, z = plane.Normal.Z, w = plane.D;
        return new KBPlane(
            x * m.M11 + y * m.M12 + z * m.M13 + w * m.M14,
            x * m.M21 + y * m.M22 + z * m.M23 + w * m.M24,
            x * m.M31 + y * m.M32 + z * m.M33 + w * m.M34,
            x * m.M41 + y * m.M42 + z * m.M43 + w * m.M44);
    }

    public static KBPlane Transform(KBPlane plane, KBQuaternion rotation)
    {
        float x2 = rotation.X + rotation.X;
        float y2 = rotation.Y + rotation.Y;
        float z2 = rotation.Z + rotation.Z;

        float wx2 = rotation.W * x2;
        float wy2 = rotation.W * y2;
        float wz2 = rotation.W * z2;
        float xx2 = rotation.X * x2;
        float xy2 = rotation.X * y2;
        float xz2 = rotation.X * z2;
        float yy2 = rotation.Y * y2;
        float yz2 = rotation.Y * z2;
        float zz2 = rotation.Z * z2;

        float m11 = 1.0f - yy2 - zz2;
        float m21 = xy2 - wz2;
        float m31 = xz2 + wy2;

        float m12 = xy2 + wz2;
        float m22 = 1.0f - xx2 - zz2;
        float m32 = yz2 - wx2;

        float m13 = xz2 - wy2;
        float m23 = yz2 + wx2;
        float m33 = 1.0f - xx2 - yy2;

        float x = plane.Normal.X, y = plane.Normal.Y, z = plane.Normal.Z;

        return new KBPlane(
            x * m11 + y * m21 + z * m31,
            x * m12 + y * m22 + z * m32,
            x * m13 + y * m23 + z * m33,
            plane.D);
    }

    public static float Dot(KBPlane plane, KBVector4 value)
    {
        return plane.Normal.X * value.X +
               plane.Normal.Y * value.Y +
               plane.Normal.Z * value.Z +
               plane.D * value.W;
    }

    public static float DotCoordinate(KBPlane plane, KBVector3 value)
    {
        return plane.Normal.X * value.X +
               plane.Normal.Y * value.Y +
               plane.Normal.Z * value.Z +
               plane.D;
    }


    public static float DotNormal(KBPlane plane, KBVector3 value)
    {
        return plane.Normal.X * value.X +
               plane.Normal.Y * value.Y +
               plane.Normal.Z * value.Z;
    }

    public static bool operator ==(KBPlane value1, KBPlane value2)
    {
        return (value1.Normal.X == value2.Normal.X &&
                value1.Normal.Y == value2.Normal.Y &&
                value1.Normal.Z == value2.Normal.Z &&
                value1.D == value2.D);
    }

    public static bool operator !=(KBPlane value1, KBPlane value2)
    {
        return (value1.Normal.X != value2.Normal.X ||
                value1.Normal.Y != value2.Normal.Y ||
                value1.Normal.Z != value2.Normal.Z ||
                value1.D != value2.D);
    }

    public override bool Equals(object obj)
    {
        if (obj is KBPlane)
        {
            return Equals((KBPlane)obj);
        }
        //
        return false;
    }
}
