using System.Threading.Tasks;
using IgiCore.Models.Player;
using IgiCore.SDK.Client.Events;
using IgiCore.SDK.Client.Rpc;
using IgiCore.SDK.Client.Services;
using IgiCore.SDK.Core.Diagnostics;

namespace Roleplay.Client
{
	public class CharacterService : Service
	{
		public CharacterService(ILogger logger, ITickManager ticks, IEventManager events, IRpcHandler rpc, User user) : base(logger, ticks, events, rpc, user)
		{
			this.Ticks.Attach(Tick);
		}

		private async Task Tick()
		{
			await Task.FromResult(0);
		}
	}
}
