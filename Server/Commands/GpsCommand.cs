using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using IgiCore.Server.Rpc;

namespace IgiCore.Server.Commands
{
	public class GpsCommand : Command
	{
		public override string Name => "gps";

		public override async Task RunCommand(Player player, List<string> args)
		{
			player
				.Event("igi:dev:gps")
				.Trigger();
		}
	}
}
