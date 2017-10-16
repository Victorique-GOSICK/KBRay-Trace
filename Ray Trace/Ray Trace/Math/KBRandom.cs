using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class KBRandom
{
    public static void Init()
    {
        _random = new Random(Int32.MaxValue);
    }

    public static Int32 Next(Int32 value0, Int32 value1)
    {
        return _random.Next(value0, value1);
    }

    public static float Next(float value0, float value1)
    {
        return _random.Next((Int32)(value0 * 100), (Int32)(value1 * 100)) * 0.01f;
    }

    static Random _random;
}
