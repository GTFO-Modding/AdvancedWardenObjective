using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWO;
internal static class RNG
{
    private readonly static Random _Rand = new();

    public static float Float01
    {
        get => (float)_Rand.NextDouble();
    }

    public static int Int0Positive
    {
        get => _Rand.Next(0, int.MaxValue);
    }

    public static int Int0Negative
    {
        get => _Rand.Next(int.MinValue, 0 + 1);
    }

    public static int Int
    {
        get => _Rand.Next(int.MinValue, int.MaxValue);
    }

    public static bool MeetProbability(float prob)
    {
        if (prob >= 1.0f)
            return true;

        if (prob <= 0.0f)
            return false;

        return prob >= Float01;
    }
}
