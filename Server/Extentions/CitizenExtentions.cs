using System.Threading.Tasks;
using IgiCore.Models.Player;
using IgiCore.Server.Controllers;
using Citizen = CitizenFX.Core.Player;
using User = IgiCore.Server.Models.Player.User;

namespace IgiCore.Server.Extentions
{
	public static class CitizenExtentions
	{
		public static async Task<Character> ToLastCharacter(this Citizen citizen) => await CharacterController.GetLatestOrCreate(await User.GetOrCreate(citizen));
	}
}
