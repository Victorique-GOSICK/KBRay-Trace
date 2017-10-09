using System;
using System.Collections.Generic;

//图元基类
public class KBPrimitive
{
    public KBPrimitive()
    {
        _primitiveID = _nextPrimitiveID++;
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

    public virtual void Refine(List<KBPrimitive> primitive)
    {

    }

    UInt32 _primitiveID;
    static UInt32 _nextPrimitiveID = 1;
}

public class KBGeometricPrimitive : KBPrimitive
{
    public override KBAABBox WorldBound()
    {
        if(_shape == null)
        {
            return null;
        }
        //
        return _shape.WorldBound();
    }

    public override bool CanIntersect()
    {
        if (_shape == null)
        {
            return false;
        }
        //
        return _shape.CanIntersect();
    }

    public override void Refine(List<KBPrimitive> primitive)
    {
        
    }

    KBShape _shape;
}