using System;
using System.Threading;

/// <summary>
/// Thread-safe random generator.
/// </summary>
public static class StaticRandom
{
    static int seed = Environment.TickCount;

    static readonly ThreadLocal<Random> random =
        new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref seed)));

    public static int Random()
    {
        return random.Value.Next();
    }
    public static int Random(int max)
    {
        return random.Value.Next(max);
    }
    public static int Random(int min, int max)
    {
        return random.Value.Next(min, max);
    }

    public static double RandomDouble()
    {
        return random.Value.NextDouble();
    }
    public static float RandomFloat()
    {
        return (float)RandomDouble();
    }
}