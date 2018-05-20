using System;
using CitizenFX.Core;
using IgiCore.SDK.Client.Helpers;

namespace IgiCore.SDK.Client.Extensions
{
    public static class VectorExtensions
    {
        public static Vector2 RotateAround(this Vector3 position, float radius, float angle) =>
            new Vector2(
                position.X + radius * (float)Math.Cos(angle * Math.PI / 180),
                position.Y - radius * (float)Math.Sin(angle * Math.PI / 180)
            );

        public static Vector3 TranslateDir(this Vector3 pos, float angleInDegrees, float distance) =>
            new Vector3(
                pos.X + (float)Math.Cos(MathUtil.DegreesToRadians(angleInDegrees)) * distance,
                pos.Y + (float)Math.Sin(MathUtil.DegreesToRadians(angleInDegrees)) * distance,
                pos.Z
            );

        public static Vector3 Lerp(Vector3 pos1, Vector3 pos2, float normalisedInterval) =>
            new Vector3(
                MathHelpers.Lerp(pos1.X, pos2.X, normalisedInterval),
                MathHelpers.Lerp(pos1.Y, pos2.Y, normalisedInterval),
                MathHelpers.Lerp(pos1.Z, pos2.Z, normalisedInterval)
            );

    }
}
