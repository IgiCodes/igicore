using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using IgiCore.Server.Controllers;
using IgiCore.Server.Extentions;

namespace IgiCore.Server.Commands
{
	public class GroupCommand : Command
	{
		public override string Name => "group";

		public override async Task RunCommand(Player player, List<string> args)
		{
			if (args[0] == null) return;

			await GroupController.Create(await player.ToLastCharacter(), args[0]);
		}
	}
}
