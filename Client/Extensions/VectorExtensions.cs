using System;
using CitizenFX.Core;

namespace IgiCore.Client.Extensions
{
	public static class VectorExtensions
	{
		public static Vector2 RotateAround(this Vector3 position, float radius, float angle)
		{
			float x = position.X + radius * (float)Math.Cos(angle * Math.PI / 180);
			float y = position.Y - radius * (float)Math.Sin(angle * Math.PI / 180);

			return new Vector2(x, y);
		}
	}
}
