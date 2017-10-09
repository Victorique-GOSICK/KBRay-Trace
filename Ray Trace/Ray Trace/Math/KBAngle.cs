using System;
using System.Collections.Generic;

public class KBAngle
{
    public static float AngleToRadian(float angle)
    {
        return angle * KBMathDefine.PI / 180.0f;
    }

    public static float RadianToAngle(float radian)
    {
        return radian * 180.0f / KBMathDefine.PI;
    }
}
