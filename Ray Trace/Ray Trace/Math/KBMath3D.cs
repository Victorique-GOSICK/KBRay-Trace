using System;
using System.Collections.Generic;

public class KBMath3D
{
    public static void CoordinateSystem(KBVector3 v1, out KBVector3 v2, out KBVector3 v3)
    {
        if (KBMathDefine.Abs(v1.X) > KBMathDefine.Abs(v1.Y))
        {
            float invLen = 1.0f / KBMathDefine.Sqrt(v1.X * v1.X + v1.Z * v1.Z);
            v2 = new KBVector3(-v1.Z * invLen, 0.0f, v1.X * invLen);
        }
        else
        {
            float invLen = 1.0f / KBMathDefine.Sqrt(v1.Y * v1.Y + v1.Z * v1.Z);
            v2 = new KBVector3(0.0f, v1.Z * invLen, -v1.Y * invLen);
        }
        //
        v3 = KBVector3.Cross(v1, v2);
    }


    //求解KBTtriangleMesh的b1.b2值，来进行插值计算
    public static bool SolveLinearSystem2x2(List<KBVector2> A, KBVector2 B, ref float x0, ref float x1)
    {
        KBVector2 A1 = A[0];
        KBVector2 A2 = A[1];
        float det = A1.X * A2.Y - A1.Y * A2.X;//求解逆矩阵
        if (KBMathDefine.Abs(det) < 1e-10f)
        {
            return false;
        }
        //
        x0 = (A2.Y * B.X - A1.Y * B.Y) / det;
        x1 = (A1.X * B.Y - A2.X * B.X) / det;
        if (float.IsNaN(x0) || float.IsNaN(x1))
        {
            return false;
        }
        //
        return true;
    }


    public static KBVector3 RandomDir_In_Unit_Sphere()
    {
        KBVector3 tagertVector;
        do
        {
            KBVector3 randomPos = new KBVector3(KBRandom.Next(0.0f, 1.0f), KBRandom.Next(0.0f, 1.0f), KBRandom.Next(0.0f, 1.0f));
            tagertVector = randomPos * 2.0f - KBVector3.ONE;
        } while (KBVector3.Dot(tagertVector, tagertVector) >= 1.0f);
        //
        return tagertVector;
    }
}
