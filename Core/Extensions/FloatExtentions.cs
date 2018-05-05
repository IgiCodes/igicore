namespace IgiCore.Core.Extensions
{
    public static class FloatExtentions
    {
        public static bool IsBetween(this float value, float min, float max) => value > min && value < max;
    }
}
