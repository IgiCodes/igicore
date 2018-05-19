using System;
using System.Linq;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using IgiCore.Core.Models.Connection;
using IgiCore.Core.Rpc;
using IgiCore.Server.Managers;
using IgiCore.Server.Rpc;

namespace IgiCore.Server.Controllers
{
    public static class ClientController
	{
		public static void Ready([FromSource] Player player)
		{
			player
				.Event(RpcEvents.GetServerInformation)
				.Attach(new ServerInformation
				{
					ResourceName = API.GetCurrentResourceName(),
					ServerName = ConfigurationManager.Configuration.ServerName,
					DateTime = DateTime.UtcNow,
					Weather = "EXTRASUNNY", // TODO
					Atms = Server.Db.BankAtms.ToList(),
					Branches = Server.Db.BankBranches.ToList()
				})
				.Trigger();
		}

		public static void Disconnect([FromSource] Player player)
		{
			player.Drop("Disconnected");
		}
	}
}
