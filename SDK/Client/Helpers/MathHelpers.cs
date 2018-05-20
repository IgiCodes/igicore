namespace IgiCore.SDK.Client.Helpers
{
    public static class MathHelpers
    {
        public static float Lerp(float v0, float v1, float interval) => (1 - interval) * v0 + interval * v1;
    }
}
