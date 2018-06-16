using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;

namespace IgiCore.SDK.Core.Models
{
	[PublicAPI]
	[ComplexType]
	public class Position
	{
		public Position() { }

		public Position(float x, float y, float z)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		[Required]
		// TODO: Range
		public float X { get; set; }

		[Required]
		// TODO: Range
		public float Y { get; set; }

		[Required]
		// TODO: Range
		public float Z { get; set; }
	}
}
