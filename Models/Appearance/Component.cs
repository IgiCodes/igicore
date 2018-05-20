using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;

namespace IgiCore.Models.Appearance
{
	[PublicAPI]
	[ComplexType]
	public class Component
	{
		[Required]
		[Range(0, 11)]
		public ComponentTypes Type { get; set; }

		[Required]
		[Range(0, 100)] // TODO
		public int Index { get; set; }

		[Required]
		[Range(0, 100)] // TODO
		public int Texture { get; set; }
    }
}
