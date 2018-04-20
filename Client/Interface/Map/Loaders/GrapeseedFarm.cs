using System.Collections.Generic;

namespace IgiCore.Client.Interface.Map.Loaders
{
	/// <summary>
	/// Loads the farmhouse in Grapeseed with a meth lab in the basement (O'Neil Ranch).
	/// Location: 2447.9, 4973.4, 47.7
	/// </summary>
	/// <seealso cref="MapLoader" />
	public class GrapeseedFarm : MapLoader
	{
		protected override IEnumerable<string> Ipls => new List<string>
		{
			"farm",
			"farmint",
			"farm_lod",
			"farm_props",
			"des_farmhouse"

			//Burnt O'Neil Ranch:
			//farm_burnt
			//farm_burnt_props

			//Remove the following IPL to open up all doors:
			//farmint_cap
		};
	}
}
