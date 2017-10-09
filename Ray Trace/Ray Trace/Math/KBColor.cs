using System;
using System.Collections.Generic;

public struct KBColor
{
    public float R;
    public float G;
    public float B;
    public float A;

    public static KBColor White = new KBColor(1, 1, 1, 1);

    public KBColor(float r, float g, float b, float a)
    {
        R = r;
        G = g;
        B = b;
        A = a;
    }

    public KBColor(float r, float g, float b)
    {
        R = r;
        G = g;
        B = b;
        A = 1;
    }
}
