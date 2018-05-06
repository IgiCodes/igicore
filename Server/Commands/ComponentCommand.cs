using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace IgiCore.Server.Commands
{
	public class ComponentCommand : Command
	{
		public override string Name => "component";

		public override async Task RunCommand(Player player, List<string> args)
		{
			BaseScript.TriggerClientEvent(
				player,
				"igi:character:component:set",
				int.Parse(args[0]),
				int.Parse(args[1]),
				int.Parse(args[2])
			);
		}
	}
}
