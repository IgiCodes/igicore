using System.ComponentModel.DataAnnotations.Schema;

namespace IgiCore.SDK.Core.Models
{
	[ComplexType]
	public class Color
	{
		public byte? R { get; set; }
		public byte? G { get; set; }
		public byte? B { get; set; }
		public byte? A { get; set; }

		public Color() { }

	}
}
