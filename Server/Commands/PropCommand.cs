using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using IgiCore.Core;
using IgiCore.Server.Rpc;

namespace IgiCore.Server.Commands
{
	public class PropCommand : Command
	{
		public override string Name => "prop";

		public override async Task RunCommand(Player player, List<string> args)
		{
			player
				.Event(RpcEvents.CharacterPropSet)
				.Attach(int.Parse(args[0]))
				.Attach(int.Parse(args[1]))
				.Attach(int.Parse(args[2]))
				.Trigger();
		}
	}
}
