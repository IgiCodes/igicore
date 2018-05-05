using System.Collections.Generic;

namespace IgiCore.Client.Interface.Map.Loaders
{
	/// <summary>
	/// Loads the interior of Trevor's trailer.
	/// Location: 1985.48132, 3828.76757, 32.5
	/// </summary>
	/// <seealso cref="MapLoader" />
	public class TrevorsTrailer : MapLoader
	{
		protected override IEnumerable<string> Ipls => new List<string>
		{
			// Two possible designs:

			"TrevorsTrailerTrash"
			//"trevorstrailertidy"
		};
	}
}
