using System;
using System.Collections.Generic;

public class KBTransform
{
    public KBMatrix4x4 ObjectToWorld;
    public KBMatrix4x4 WorldToObject;

    public void RayWorldToObject(KBRay ray, out KBRay newRay)
    {
        KBVector3 pos = KBMatrix4x4.Multiply(WorldToObject, ray.Origin);
        newRay = new KBRay(pos, ray.Direction);
    }
}
