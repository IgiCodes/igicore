using System.Collections.Generic;

namespace IgiCore.Client.Interface.Map.Loaders
{
	/// <summary>
	/// Loads the cargo ship at the docks.
	/// Location: -90.0, -2365.8, 14.3
	/// </summary>
	/// <seealso cref="MapLoader" />
	public class CargoShip : MapLoader
	{
		protected override IEnumerable<string> Ipls => new List<string>
		{
			"cargoship"
		};
	}
}
