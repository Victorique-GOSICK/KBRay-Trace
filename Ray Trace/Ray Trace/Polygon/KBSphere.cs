using System;
using System.Collections.Generic;

public class KBSphere : KBShape
{

    public float Radius;
    public float ZMin;
    public float ZMax;
    public float PhiMax;
    public float thetaMin;
    public float thetaMax;

    public KBSphere(KBTransform tran, float radius, float zmin, float zmax, float pm) : base(tran)
    {
        Radius = radius;
        ZMin = zmin;
        ZMax = zmax;
        thetaMin = KBMathDefine.Acos(KBMathDefine.Clamp(ZMin / radius, -1.0f, 1.0f));
        thetaMax = KBMathDefine.Acos(KBMathDefine.Clamp(zmax / radius, -1.0f, 1.0f));
        PhiMax = KBAngle.AngleToRadian(KBMathDefine.Clamp(pm, 0.0f, 360.0f));
    }

    public override KBAABBox ObjectBound()
    {
        KBVector3 minPoint = new KBVector3(-Radius, -Radius, ZMin);
        KBVector3 maxPoint = new KBVector3(Radius, Radius, ZMax);
        KBAABBox objectBound = new KBAABBox(minPoint, maxPoint);
        return objectBound;
    }

    public override bool Intersect(KBRay ray, out float tHit, out float rayEpsilon, out DifferentialGeometry geometry)
    {
        //
        // ray : o + t * d;
        // sphere : x^2 + y^2 + z^2 = r^2
        // (o + t * dx)^2 + (o + t * dy)^2 + (o + t * dz)^2 = r^2
        //
        geometry = null;
        tHit = 0.0f;
        rayEpsilon = 0.0f;
        float phi;
        KBRay newRay;
        Transform.RayWorldToObject(ray, out newRay);
        KBVector3 dir = newRay.Direction;
        KBVector3 origin = newRay.Origin;
        //
        float A = dir.X * dir.X + dir.Y * dir.Y + dir.Z * dir.Z;
        float B = 2 * (dir.X * origin.X + dir.Y * origin.Y + dir.Z * origin.Z);
        float C = origin.X * origin.X + origin.Y * origin.Y + origin.Z * origin.Z - Radius * Radius;
        //
        float value01 = 0.0f;
        float value02 = 0.0f;
        if (!KBMathDefine.Quadratic(A, B, C, out value01, out value02))
        {
            return false;
        }
        //
        if (value01 > newRay.Max || value02 < newRay.Min)
        {
            return false;
        }
        //
        float thit = value01;
        if (value01 < newRay.Min)
        {
            thit = value02;
            if (value02 > newRay.Max)
            {
                return false;
            }
        }
        //
        KBVector3 hitPoint = newRay.GetPoint(thit);
        if (hitPoint.X == 0.0f && hitPoint.Y == 0.0f)
        {
            hitPoint.X = 1e-5f * Radius;
        }
        phi = KBMathDefine.Atan2(hitPoint.Y, hitPoint.X);
        if (phi < 0.0f)
        {
            phi += 2 * KBMathDefine.PI;
        }
        if ((ZMin > -Radius && hitPoint.Z < ZMin) || (ZMax < Radius && hitPoint.Z > ZMax) || phi > PhiMax)
        {
            if (thit == value02)
            {
                return false;
            }
            if (thit > newRay.Max)
            {
                return false;
            }
            //
            thit = value02;
            hitPoint = newRay.GetPoint(thit);
            if (hitPoint.X == 0.0f && hitPoint.Y == 0.0f)
            {
                hitPoint.X = 1e-5f * Radius;
            }
            phi = KBMathDefine.Atan2(hitPoint.Y, hitPoint.X);
            if (phi < 0.0f)
            {
                phi += 2 * KBMathDefine.PI;
            }
            if ((ZMin > -Radius && hitPoint.Z < ZMin) || (ZMax < Radius && hitPoint.Z > ZMax) || phi > PhiMax)
            {
                return false;
            }
        }
        //
        //计算该点的UV值
        float u = phi / PhiMax; //u mapping to [0 1]
        float theta = KBMathDefine.Acos(KBMathDefine.Clamp(hitPoint.Z / Radius, -1.0f, 1.0f));
        float v = (theta - thetaMin) / (thetaMax - thetaMin);  //v mapping to [0 1]

        //计算参数偏导数
        float zradius = KBMathDefine.Sqrt(hitPoint.X * hitPoint.X + hitPoint.Y * hitPoint.Y);
        float invzradius = 1.0f / zradius;
        float cosphi = hitPoint.X * invzradius;
        float sinphi = hitPoint.Y * invzradius;
        KBVector3 dpdu = new KBVector3(-PhiMax * hitPoint.Y, PhiMax * hitPoint.X, 0);
        KBVector3 dpdv = (thetaMax - thetaMin) * new KBVector3(hitPoint.Z * cosphi, hitPoint.Z * sinphi, -Radius * KBMathDefine.Sin(theta));

        //计算表面法线偏导数(微分几何weingarten公式）
        KBVector3 d2Pduu = -PhiMax * PhiMax * new KBVector3(hitPoint.X, hitPoint.Y, 0);
        KBVector3 d2Pduv = (thetaMax - thetaMin) * hitPoint.Z * PhiMax * new KBVector3(-sinphi, cosphi, 0.0f);
        KBVector3 d2Pdvv = -(thetaMax - thetaMin) * (thetaMax - thetaMin) * new KBVector3(hitPoint.X, hitPoint.Y, hitPoint.Z);
        float E = KBVector3.Dot(dpdu, dpdu);
        float F = KBVector3.Dot(dpdu, dpdv);
        float G = KBVector3.Dot(dpdv, dpdv);
        KBVector3 N = KBVector3.Normalize(KBVector3.Cross(dpdu, dpdv));
        float e = KBVector3.Dot(N, d2Pduu);
        float f = KBVector3.Dot(N, d2Pduv);
        float g = KBVector3.Dot(N, d2Pdvv);
        float invEGF2 = 1.0f / (E * G - F * F);
        KBVector3 dndu = new KBVector3((f * F - e * G) * invEGF2 * dpdu + (e * F - f * E) * invEGF2 * dpdv);
        KBVector3 dndv = new KBVector3((g * F - f * G) * invEGF2 * dpdu + (f * F - g * E) * invEGF2 * dpdv);
        geometry = new DifferentialGeometry(KBMatrix4x4.Multiply(Transform.ObjectToWorld, hitPoint), KBMatrix4x4.Multiply(Transform.ObjectToWorld, dpdu), 
            KBMatrix4x4.Multiply(Transform.ObjectToWorld, dpdv), KBMatrix4x4.Multiply(Transform.ObjectToWorld, dndu), KBMatrix4x4.Multiply(Transform.ObjectToWorld, dndv), u, v, this);
        tHit = thit;
        rayEpsilon = tHit * 5e-4f;//?
        //
        return true;
    }

    //第一类型曲面积分
    public override float Area()
    {
        return PhiMax * Radius * (ZMax - ZMin);
    }
}