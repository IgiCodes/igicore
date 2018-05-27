using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;

namespace IgiCore.Models
{
	[PublicAPI]
	[ComplexType]
	public class Position
	{
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
