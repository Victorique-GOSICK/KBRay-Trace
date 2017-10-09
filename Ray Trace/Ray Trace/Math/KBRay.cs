using System;
using System.Collections.Generic;

public class KBRay
{
    public KBVector3 Origin;
    public KBVector3 Direction;
    public float Min = 0.0f;
    public float Max = float.MaxValue;

    public KBRay(KBVector3 origin, KBVector3 direction)
    {
        Origin = origin;
        Direction = direction;
    }

    public KBVector3 GetPoint(float distance)
    {
        return Origin + Direction * distance;
    }
}
