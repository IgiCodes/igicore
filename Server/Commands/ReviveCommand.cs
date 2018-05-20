using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using IgiCore.Core.Rpc;
using IgiCore.Server.Rpc;

namespace IgiCore.Server.Commands
{
	public class ReviveCommand : Command
	{
		public override string Name => "revive";

		public override async Task RunCommand(Player player, List<string> args) => player.Event(RpcEvents.CharacterRevive).Trigger();
	}
}
