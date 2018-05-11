using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using IgiCore.Core;
using IgiCore.Core.Rpc;
using IgiCore.Server.Rpc;

namespace IgiCore.Server.Commands
{
	public class ComponentCommand : Command
	{
		public override string Name => "component";

		public override async Task RunCommand(Player player, List<string> args)
		{
			player
				.Event(RpcEvents.CharacterComponentSet)
				.Attach(int.Parse(args[0]))
				.Attach(int.Parse(args[1]))
				.Attach(int.Parse(args[2]))
				.Trigger();
		}
	}
}
