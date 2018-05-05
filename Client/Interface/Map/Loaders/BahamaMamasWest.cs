using System.Collections.Generic;

namespace IgiCore.Client.Interface.Map.Loaders
{
	/// <summary>
	/// Loads the inaccessable "Bahama Mamas West" bar interior.
	/// Location: -1391, -591.54, 30.35
	/// </summary>
	/// <seealso cref="MapLoader" />
	public class BahamaMamasWest : MapLoader
	{
		protected override IEnumerable<string> Ipls => new List<string>
		{
			"hei_sm_16_interior_v_bahama_milo_"
		};
	}
}
