using System;
using System.Collections.Generic;

//三角网格
public class KBTtriangleMesh : KBShape
{
    public List<KBVector3> Vertexs;
    public List<Int32> Indexs;
    public Int32 VerextCount;
    public Int32 FaceCount;
    public List<KBVector3> Normals;
    public List<KBVector3> Tangents;
    public List<KBVector2> UVs;

    public KBTtriangleMesh(KBTransform tran, Int32 faceCount, Int32 verextCount, List<Int32> indexs,
        List<KBVector3> vertexs, List<KBVector3> normals, List<KBVector3> tangents, List<KBVector2> uvs) : base(tran)
    {
        Vertexs = vertexs;
        FaceCount = faceCount;
        VerextCount = verextCount;
        Indexs = indexs;
        Normals = normals;
        Tangents = tangents;
        UVs = uvs;
        for (Int32 iter = 0; iter < verextCount; ++iter)
        {
            KBVector3 iterPos = Vertexs[iter];
            Vertexs[iter] = KBMatrix4x4.Multiply(tran.ObjectToWorld, iterPos);
        }
    }

    public override KBAABBox ObjectBound()
    {
        KBAABBox objectBound = new KBAABBox();
        for (Int32 iter = 0; iter < VerextCount; ++iter)
        {
            KBVector3 iterPos = KBMatrix4x4.Multiply(Transform.WorldToObject, Vertexs[iter]);
            objectBound.AddPos(iterPos);
        }
        //
        return objectBound;
    }

    public override KBAABBox WorldBound()
    {
        KBAABBox worldBound = new KBAABBox();
        for (Int32 iter = 0; iter < VerextCount; ++iter)
        {
            worldBound.AddPos(Vertexs[iter]);
        }
        //
        return worldBound;
    }

    //三角网格分割为多个三角形
    public override void Refine(List<KBShape> shapes)
    {
        for (Int32 iter = 0; iter < FaceCount; ++iter)
        {
            shapes.Add(new KBTtriangle(Transform, this, iter));
        }
    }
}


public class KBTtriangle : KBShape
{
    public Int32 StartIndex;
    public KBTtriangleMesh Mesh;

    public KBTtriangle(KBTransform tran, KBTtriangleMesh mesh, Int32 index) : base(tran)
    {
        Mesh = mesh;
        StartIndex = Mesh.Indexs[3 * index];
    }

    public override KBAABBox ObjectBound()
    {
        KBAABBox objectBound = new KBAABBox();
        objectBound.AddPos(KBMatrix4x4.Multiply(Transform.WorldToObject, Mesh.Vertexs[StartIndex]));
        objectBound.AddPos(KBMatrix4x4.Multiply(Transform.WorldToObject, Mesh.Vertexs[StartIndex + 1]));
        objectBound.AddPos(KBMatrix4x4.Multiply(Transform.WorldToObject, Mesh.Vertexs[StartIndex + 2]));
        return objectBound;
    }

    public override KBAABBox WorldBound()
    {
        KBAABBox worldBound = new KBAABBox();
        worldBound.AddPos(Mesh.Vertexs[StartIndex]);
        worldBound.AddPos(Mesh.Vertexs[StartIndex + 1]);
        worldBound.AddPos(Mesh.Vertexs[StartIndex + 2]);
        return worldBound;
    }

    //o + t * d = (1-b1-b2) * p1 + b1 * p2 + b2 * p3 (b1 + b2 < 1.0f)
    //克莱默法则 求解方程的解
    public override bool Intersect(KBRay ray, out float tHit, out float rayEpsilon, out DifferentialGeometry geometry)
    {
        tHit = 0.0f;
        rayEpsilon = 0.0f;
        geometry = null;
        KBVector3 p1 = Mesh.Vertexs[StartIndex];
        KBVector3 p2 = Mesh.Vertexs[StartIndex + 1];
        KBVector3 p3 = Mesh.Vertexs[StartIndex + 2];
        KBVector3 e1 = p2 - p1;
        KBVector3 e2 = p3 - p1;
        KBVector3 s1 = KBVector3.Cross(ray.Direction, e2);
        float divisor = KBVector3.Dot(s1, e1);
        if (divisor == 0.0f)//行列式值为0，是奇异矩阵，没有解或是无穷解
        {
            return false;
        }
        //
        float invDivisor = 1.0f / divisor;
        KBVector3 s = ray.Origin - p1;
        float b1 = KBVector3.Dot(s, s1) * invDivisor;
        if (b1 < 0.0f || b1 > 1.0f) //加权平均 权值相加为1
        {
            return false;
        }
        //
        KBVector3 s2 = KBVector3.Cross(s, e1);
        float b2 = KBVector3.Dot(ray.Direction, s2) * invDivisor;
        if (b2 < 0.0f || b1 + b2 > 1.0f)
        {
            return false;
        }
        //
        float t = KBVector3.Dot(e2, s2) * invDivisor;
        if (t < ray.Min || t > ray.Max)
        {
            return false;
        }
        //
        KBVector3 dpdu;
        KBVector3 dpdv;
        List<KBVector2> uvs = new List<KBVector2>();
        uvs.Capacity = 3;
        GetUVs(uvs);
        KBVector2 uv1 = uvs[0];
        KBVector2 uv2 = uvs[1];
        KBVector2 uv3 = uvs[2];
        float du1 = uv1.X - uv3.X;
        float du2 = uv2.X - uv3.X;
        float dv1 = uv1.Y - uv3.Y;
        float dv2 = uv2.Y - uv3.Y;
        KBVector3 dp1 = p1 - p3, dp2 = p2 - p3;
        float determinant = du1 * dv2 - dv1 * du2;
        if (determinant == 0.0f) //没有逆矩阵，奇异矩阵
        {
            KBMath3D.CoordinateSystem(KBVector3.Cross(e2, e1), out dpdu, out dpdv);
        }
        else
        {
            float invdet = 1.0f / determinant;
            dpdu = (dv2 * dp1 - dv1 * dp2) * invdet;
            dpdv = (-du2 * dp1 + du1 * dp2) * invdet;
        }
        //
        float b0 = 1 - b1 - b2;
        float tu = b0 * uv1.X + b1 * uv2.X + b2 * uv3.X;
        float tv = b0 * uv1.Y + b1 * uv2.Y + b2 * uv3.Y;
        KBVector3 hitPoint = ray.Origin + t * ray.Direction;
        geometry = new DifferentialGeometry(hitPoint, dpdu, dpdv, new KBVector3(0, 0, 0), new KBVector3(0, 0, 0), tu, tv, this);
        tHit = t;
        rayEpsilon = 1e-3f * tHit;
        return true;
    }

    public void GetUVs(List<KBVector2> uvs)
    {
        if (uvs == null)
        {
            return;
        }
        //
        KBVector2 uv1 = new KBVector2(0.0f, 0.0f);
        KBVector2 uv2 = new KBVector2(0.0f, 0.0f);
        KBVector2 uv3 = new KBVector2(0.0f, 0.0f);
        if (Mesh.UVs.Count >= 3)
        {
            uv1 = Mesh.UVs[StartIndex];
            uv2 = Mesh.UVs[StartIndex + 1];
            uv3 = Mesh.UVs[StartIndex + 2];
        }
        //
        uvs.Add(uv1);
        uvs.Add(uv2);
        uvs.Add(uv3);
    }

    public override float Area()
    {
        KBVector3 p1 = Mesh.Vertexs[StartIndex];
        KBVector3 p2 = Mesh.Vertexs[StartIndex + 1];
        KBVector3 p3 = Mesh.Vertexs[StartIndex + 2];
        KBVector3 e1 = p2 - p1;
        KBVector3 e2 = p3 - p1;
        return 0.5f * KBVector3.Cross(e1, e2).Magnitude();
    }

    public void GetShadingGeometry(KBTransform tran, DifferentialGeometry dg, out DifferentialGeometry dgShaing)
    {
        dgShaing = null;
        if ((Mesh.Normals == null || Mesh.Normals.Count == 0) && (Mesh.Tangents == null || Mesh.Tangents.Count == 0))
        {
            dgShaing = dg;
            return;
        }
        //
        float[] b = new float[3];
        List<KBVector2> uvs = new List<KBVector2>();
        uvs.Capacity = 3;
        GetUVs(uvs);
        KBVector2 uv1 = uvs[0];
        KBVector2 uv2 = uvs[1];
        KBVector2 uv3 = uvs[2];
        //
        //|u1-u0,u2-u1| |b1|   |u-u0|
        //|           | |  | = |    |
        //|v1-v0,v2-v1| |b2|   |v-v0|
        //
        List<KBVector2> A = new List<KBVector2>(2);
        A.Add(new KBVector2(uv2.X - uv1.X, uv3.X - uv2.X));
        A.Add(new KBVector2(uv2.Y - uv1.Y, uv3.Y - uv2.Y));
        KBVector2 B = new KBVector2(dg.U - uv1.X, dg.V - uv1.Y);
        float b0 = 0.0f;
        float b1 = 0.0f;
        float b2 = 0.0f;
        if (KBMath3D.SolveLinearSystem2x2(A, B, ref b1, ref b2)) //判断b1和b2是否存在
        {
            b0 = 1.0f - b1 - b2;
        }
        else
        {
            b0 = b1 = b2 = 1.0f / 3.0f;//不存在，就质心
        }
        //
        KBVector3 normal;
        KBVector3 tangent;
        KBVector3 bitangent;
        if (Mesh.Normals == null || Mesh.Normals.Count == 0)
        {
            normal = KBVector3.Normalize(dg.Normal); //导入的mesh没有normal，则取自己算的那个法线
        }
        else
        {
            //三个点的法线进行加权平均
            KBVector3 normal01 = Mesh.Normals[StartIndex];
            KBVector3 normal02 = Mesh.Normals[StartIndex + 1];
            KBVector3 normal03 = Mesh.Normals[StartIndex + 2];
            normal = KBVector3.Normalize(KBMatrix4x4.Multiply(tran.ObjectToWorld, b0 * normal01 + b1 * normal02 + b2 * normal03));
        }
        //
        if (Mesh.Tangents == null || Mesh.Tangents.Count == 0)
        {
            tangent = KBVector3.Normalize(dg.DPDU);
        }
        else
        {
            KBVector3 tangent01 = Mesh.Tangents[StartIndex];
            KBVector3 tangent02 = Mesh.Tangents[StartIndex + 1];
            KBVector3 tangent03 = Mesh.Tangents[StartIndex + 2];
            tangent = KBVector3.Normalize(KBMatrix4x4.Multiply(tran.ObjectToWorld, b0 * tangent01 + b1 * tangent02 + b2 * tangent03));
        }
        //
        bitangent = KBVector3.Cross(tangent, normal);
        if (bitangent.LengthSquared() > 0.0f)
        {
            bitangent = KBVector3.Normalize(bitangent);
            tangent = KBVector3.Cross(bitangent, normal);//防止不垂直的现象
        }
        else
        {
            KBMath3D.CoordinateSystem(normal, out tangent, out bitangent);
        }
        //
        KBVector3 dndu;
        KBVector3 dndv;
        if (Mesh.Normals == null || Mesh.Normals.Count == 0)
        {
            List<KBVector2> inuvs = new List<KBVector2>();
            uvs.Capacity = 3;
            GetUVs(uvs);
            KBVector2 inuv1 = uvs[0];
            KBVector2 inuv2 = uvs[1];
            KBVector2 inuv3 = uvs[2];
            float du1 = inuv1.X - inuv3.X;
            float du2 = inuv2.X - inuv3.X;
            float dv1 = inuv1.Y - inuv3.Y;
            float dv2 = inuv2.Y - inuv3.Y;
            KBVector3 dn1 = Mesh.Normals[StartIndex] - Mesh.Normals[StartIndex + 2];
            KBVector3 dn2 = Mesh.Normals[StartIndex + 1] - Mesh.Normals[StartIndex + 2];
            float determinant = du1 * dv2 - dv1 * du2;
            if (determinant == 0.0f)
            {
                dndu = dndv = new KBVector3(0, 0, 0);
            }
            else
            {
                float invdet = 1.0f / determinant;
                dndu = (dv2 * dn1 - dv1 * dn2) * invdet;
                dndv = (-du2 * dn1 + du1 * dn2) * invdet;
            }
        }
        else
        {
            dndu = dndv = new KBVector3(0, 0, 0);
        }
        //
        dgShaing = new DifferentialGeometry(dg.HitPoint, tangent, bitangent, KBMatrix4x4.Multiply(tran.ObjectToWorld, dndu), KBMatrix4x4.Multiply(tran.ObjectToWorld, dndv), dg.U, dg.V, dg.Shape);
        dgShaing.DUDX = dg.DUDX;
        dgShaing.DUDY = dg.DUDY;
        dgShaing.DVDX = dg.DVDX;
        dgShaing.DVDY = dg.DVDY;
        dgShaing.DPDX = dg.DPDX;
        dgShaing.DPDY = dg.DPDY;
    }
}