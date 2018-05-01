using System.Threading.Tasks;
using IgiCore.Server.Models.Player;
using Citizen = CitizenFX.Core.Player;

namespace IgiCore.Server.Extentions
{
	public static class CitizenExtentions
	{
		public static async Task<Character> ToLastCharacter(this Citizen citizen) => await Character.GetLatestOrCreate(await User.GetOrCreate(citizen));
	}
}
