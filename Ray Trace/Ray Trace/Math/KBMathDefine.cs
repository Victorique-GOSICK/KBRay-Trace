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
        return (1 - value) * from  + to * value;
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
        if(value < min)
        {
            value = min; 
        }
        else if(value > max)
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

    //求根公式求解
    public static bool Quadratic(float A, float B, float C, out float value01, out float value02)
    {
        float discrim = B * B - 4.0f * A * C;
        if(discrim < 0.0f)
        {
            value01 = 0.0f;
            value02 = 0.0f;
            return false;
        }
        //
        float rootDiscrim = Sqrt(discrim);
        float q;
        if (B < 0)
        {
            q = -.5f * (B - rootDiscrim);
        }
        else
        {
            q = -.5f * (B + rootDiscrim);
        }
        //
        value01 = q / A;
        value02 = C / q;
        //t2 > t1
        if (value01 > value02)
        {
            float temp = value01;
            value01 = value02;
            value02 = value01;
        }
        //
        return true;
    }
}
