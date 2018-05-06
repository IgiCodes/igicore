using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace IgiCore.Server.Commands
{
	public class GpsCommand : Command
	{
		public override string Name => "gps";

		public override async Task RunCommand(Player player, List<string> args)
		{
			BaseScript.TriggerClientEvent(player, "igi:user:gps");
		}
	}
}
