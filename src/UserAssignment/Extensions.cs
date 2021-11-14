namespace UserAssignment;
public static class Extensions
{
    /// <summary>
    /// This extension method will allow to set lower/upper bounds to generate a random double.
    /// </summary>
    /// <param name="random"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static double NextDouble(this Random random, double min, double max)
    {
        return random.NextDouble() * (max - min) + min;
    }
}
