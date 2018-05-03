using System.Collections.Generic;
using System.Linq;
using IgiCore.Server.Models.Player;

namespace IgiCore.Server.Extentions
{
	public static class CharacterExtentions
	{
		public static IEnumerable<Character> NotDeleted(this IEnumerable<Character> source) => source.Where(c => c.Deleted == null);
	}
}
