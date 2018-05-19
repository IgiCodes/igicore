using System;
using System.Data.Entity.Migrations;
using CitizenFX.Core;
using IgiCore.Core.Rpc;
using IgiCore.Server.Models.Player;
using IgiCore.Server.Rpc;

namespace IgiCore.Server.Controllers
{
    public static class UserController
	{
		public static async void AcceptRules([FromSource] Player player, string json)
		{
			var response = RpcResponse<DateTime>.Parse(json);

			var user = await User.GetOrCreate(player);

			user.AcceptedRules = response.Result;

			Server.Db.Users.AddOrUpdate(user);
			await Server.Db.SaveChangesAsync();
		}

		public static async void Load([FromSource] Player player)
		{
			player
				.Event(RpcEvents.GetUser)
				.Attach(await User.GetOrCreate(player))
				.Trigger();
		}
	}
}
