using System.Collections.Generic;

namespace IgiCore.Client.Interface.Map.Loaders
{
	/// <summary>
	/// Loads the yacht.
	/// Location: -2045.8, -1031.2, 11.9
	/// </summary>
	/// <seealso cref="MapLoader" />
	public class Yacht : MapLoader
	{
		protected override IEnumerable<string> Ipls => new List<string>
		{
			"smboat"
		};
	}
}
