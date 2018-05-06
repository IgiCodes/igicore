using System.Data.Entity.Migrations;
using CitizenFX.Core;
using IgiCore.Server.Models.Player;
using Citizen = CitizenFX.Core.Player;

namespace IgiCore.Server
{
	public partial class Server
	{
		private static async void OnPlayerConnecting([FromSource] Citizen citizen, string playerName, CallbackDelegate kickReason)
		{
			var user = await User.GetOrCreate(citizen);

			user.Name = citizen.Name;

			Db.Users.AddOrUpdate(user);
			await Db.SaveChangesAsync();

			var session = await Session.Create(citizen, user);

			Log($"[CONNECT] [{session.Id}] Player \"{user.Name}\" connected from {session.IpAddress}");
		}

		private static async void OnPlayerDropped([FromSource] Citizen citizen, string disconnectMessage, CallbackDelegate kickReason)
		{
			var user = await User.GetOrCreate(citizen);

			var session = await Session.End(user, disconnectMessage);

			Log($"[DISCONNECT] [{session.Id}] Player \"{user.Name}\" disconnected: {disconnectMessage}");
		}
	}
}
