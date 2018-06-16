using System.Drawing;
using System.Runtime.Remoting.Messaging;

namespace IgiCore.SDK.Client.Extensions
{
	public static class ColorExtensions
	{
		public static Core.Models.Color ToColor(this System.Drawing.Color color) => new Core.Models.Color
		{
			R = color.R,
			G = color.G,
			B = color.B,
			A = color.A,
		};

		public static Color ToCitColor(this Core.Models.Color color) => Color.FromArgb(color.A ?? 0, color.R ?? 0, color.G ?? 0, color.B ?? 0);
	}
}
