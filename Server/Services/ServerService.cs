using System.Threading.Tasks;
using IgiCore.Core.Services;
using IgiCore.Models.Player;

namespace IgiCore.Server.Services
{
	public abstract class ServerService : Service, IServerService
	{
		public override async Task Initialize() => await Task.FromResult(0);

		public virtual async Task<Character> OnCharacterCreate(Character character) => character;
	}
}
