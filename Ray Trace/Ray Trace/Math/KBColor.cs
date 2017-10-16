using System;
using System.Collections.Generic;

public struct KBColor
{
    public float R;
    public float G;
    public float B;
    public float A;

    public static KBColor White = new KBColor(1, 1, 1, 1);
    public static KBColor Black = new KBColor(0, 0, 0, 0);

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

    public static KBColor operator +(KBColor a, KBColor b)
    {
        return new KBColor(a.R + b.R, a.G + b.G, a.B + b.B);
    }

    public static KBColor operator -(KBColor a, KBColor b)
    {
        return new KBColor(a.R - b.R, a.G - b.G, a.B - b.B);
    }

    public static KBColor operator *(KBColor a, KBColor b)
    {
        return new KBColor(a.R * b.R, a.G * b.G, a.B * b.B);
    }

    public static KBColor operator /(KBColor a, float d)
    {
        return new KBColor(a.R / d, a.G / d, a.B / d);
    }

    public static KBColor operator *(KBColor a, float d)
    {
        return new KBColor(a.R * d, a.G * d, a.B * d);
    }
}
