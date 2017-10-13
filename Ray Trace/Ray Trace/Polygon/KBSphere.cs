using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class KBSphere : KBShape
{
    public KBVector3 Center;
    public float Radius;

    public KBSphere(KBVector3 center, float radius)
    {
        Center = center;
        Radius = radius;
    }

    public override bool Intersect(KBRay ray, float minDistance, float maxDistance, ref IntersectParams intersectParams)
    {
        KBVector3 oc = ray.Origin - Center;
        float a = KBVector3.Dot(ray.Direction, ray.Direction);
        float b = 2.0f * KBVector3.Dot(oc, ray.Direction);
        float c = KBVector3.Dot(oc, oc) - Radius * Radius;
        float discrminant = b * b - 4 * a * c;
        if(discrminant < 0)
        {
            return false;
        }
        //
        float t = (-b - KBMathDefine.Sqrt(discrminant)) / (2 * a);
        intersectParams.T = t;
        if (t < maxDistance && t> minDistance)
        {
            intersectParams.Point = ray.Origin + t * ray.Direction;
            intersectParams.Normal = (intersectParams.Point - Center).Normalize();
            return true;
        }
        //
        t = (-b + KBMathDefine.Sqrt(discrminant)) / (2 * a);
        intersectParams.T = t;
        if (t < maxDistance && t > minDistance)
        {
            intersectParams.Point = ray.Origin + t * ray.Direction;
            intersectParams.Normal = (intersectParams.Point - Center).Normalize(); ;
            return true;
        }
        //
        return false;
    }
}
