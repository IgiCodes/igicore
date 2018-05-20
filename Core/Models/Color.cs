using System.ComponentModel.DataAnnotations.Schema;

namespace IgiCore.Core.Models
{
	[ComplexType]
	public class Color
	{
		public byte? R { get; set; }
		public byte? G { get; set; }
		public byte? B { get; set; }
		public byte? A { get; set; }

		public Color() { }

		public Color(System.Drawing.Color color)
		{
			this.R = color.R;
			this.G = color.G;
			this.B = color.B;
			this.A = color.A;
		}

		public static implicit operator Color(System.Drawing.Color color)
		{
			return new Color
			{
				R = color.R,
				G = color.G,
				B = color.B,
				A = color.A,
			};
		}

		public static implicit operator System.Drawing.Color(Color color) => System.Drawing.Color.FromArgb(color.A ?? 0, color.R ?? 0, color.G ?? 0, color.B ?? 0);
	}

	
}
