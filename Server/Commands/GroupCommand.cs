using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using IgiCore.Server.Extentions;
using IgiCore.Server.Models.Groups;

namespace IgiCore.Server.Commands
{
	public class GroupCommand : Command
	{
		public override string Name => "group";

		public override async Task RunCommand(Player player, List<string> args)
		{
			if (args[0] == null) return;

			await Group.Create(await player.ToLastCharacter(), args[0]);
		}
	}
}
