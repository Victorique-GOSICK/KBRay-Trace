using System;
using System.Collections.Generic;

public class KBMathDefine
{
    public static readonly float PI = (float)Math.PI;

    public static float Abs(float value)
    {
        return Math.Abs(value);
    }

    public static float Sin(float angle)
    {
        return (float)Math.Sin(angle);
    }

    public static float Asin(float value)
    {
        return (float)Math.Abs(value);
    }

    public static float Cos(float angle)
    {
        return (float)Math.Cos(angle);
    }

    public static float Acos(float value)
    {
        return (float)Math.Acos(value);
    }

    public static float Lerp(float from, float to, float value)
    {
        return (1 - value) * from + to * value;
    }

    public static float Sqrt(float value)
    {
        return (float)Math.Sqrt((double)value);
    }

    public static float Tan(float x)
    {
        return (float)Math.Tan(x);
    }

    public static float Atan(float x)
    {
        return (float)Math.Atan(x);
    }

    //[-pi/2, pi/2]
    public static float Atan2(float y, float x)
    {
        return (float)Math.Atan2(y, x);
    }

    public static float Exp(float value)
    {
        return (float)Math.Exp(value);
    }

    public static float Max(float value0, float value1)
    {
        return Math.Max(value0, value1);
    }

    public static float Min(float value0, float value1)
    {
        return Math.Min(value0, value1);
    }

    public static float Clamp(float value, float min, float max)
    {
        if (value < min)
        {
            value = min;
        }
        else if (value > max)
        {
            value = max;
        }
        //
        return value;
    }

    public static Int32 Floor2Int(float value)
    {
        return (Int32)Math.Floor(value);
    }

    public static float Pow(float value, float count)
    {
        return (float)Math.Pow(value, count);
    }

    //https://en.wikipedia.org/wiki/Schlick%27s_approximation
    public float SchlickApproximation(float cosine, float ref_idex)
    {
        float r0 = (1 - ref_idex) / (1 + ref_idex);
        r0 = r0 * r0;
        return r0 + (1 - r0) * KBMathDefine.Pow(r0, 5.0f);
    }
}
