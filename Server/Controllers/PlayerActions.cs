using System.Data.Entity.Migrations;
using CitizenFX.Core;
using IgiCore.Server.Models.Player;

namespace IgiCore.Server.Controllers
{
    public static class PlayerActions
	{
		public static async void Connecting([FromSource] Player player, string playerName, CallbackDelegate kickReason)
		{
			var user = await User.GetOrCreate(player);

			user.Name = player.Name;

			Server.Db.Users.AddOrUpdate(user);
			await Server.Db.SaveChangesAsync();

			var session = await Session.Create(player, user);

			Server.Log($"[CONNECT] [{session.Id}] Player \"{user.Name}\" connected from {session.IpAddress}");
		}

		public static async void Dropped([FromSource] Player player, string disconnectMessage, CallbackDelegate kickReason)
		{
			var user = await User.GetOrCreate(player);

			var session = await Session.End(user, disconnectMessage);

			Server.Log($"[DISCONNECT] [{session.Id}] Player \"{user.Name}\" disconnected: {disconnectMessage}");
		}
	}
}
