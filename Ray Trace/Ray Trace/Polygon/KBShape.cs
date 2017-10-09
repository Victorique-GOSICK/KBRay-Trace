using System;
using System.Collections.Generic;

public class KBShape
{
    public KBTransform Transform;

    public KBShape(KBTransform tran)
    {
        Transform = tran;
        _ShapeID = _nextShapeID++;
    }

    public virtual KBAABBox ObjectBound()
    {
        KBAABBox objectBound = new KBAABBox();
        return objectBound;
    }

    public virtual KBAABBox WorldBound()
    {
        KBAABBox worldBound = new KBAABBox();
        return worldBound;
    }

    public virtual bool CanIntersect()
    {
        return true;
    }

    public virtual bool Intersect(KBRay ray, out float tHit, out float rayEpsilon, out DifferentialGeometry geometry)
    {
        geometry = null;
        tHit = 0.0f;
        rayEpsilon = 0.0f;
        return true;
    }

    public virtual float Area()
    {
        return 0.0f;
    }

    public virtual void Refine(List<KBShape> shapes)
    {

    }

    UInt32 _ShapeID;
    static UInt32 _nextShapeID = 1;
}
