using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWO;
internal static class RandomExtensions
{
    public static bool MeetProbability(this Random rand, float prob)
    {
        if (prob >= 1.0f)
            return true;

        if (prob <= 0.0f)
            return false;

        return prob >= rand.NextFloat01();
    }

    public static float NextRange(this Random rand, float min, float max)
    {
        return (rand.NextFloat01() * (max - min)) + min;
    }

    public static float NextFloat01(this Random rand)
    {
        return (float)rand.NextDouble();
    }
}
