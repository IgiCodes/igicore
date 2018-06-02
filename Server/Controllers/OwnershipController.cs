using System;
using System.Data.Entity.Migrations;
using System.Linq;
using CitizenFX.Core;
using IgiCore.Core.Models.Objects;
using IgiCore.Core.Rpc;
using IgiCore.Server.Models.Player;
using IgiCore.Server.Rpc;

namespace IgiCore.Server.Controllers
{
    public static class OwnershipController
	{
		public static void TransferObject<T>([FromSource] Player player, string json) where T : class, IObject
		{
			var response = RpcResponse<T>.Parse(json);

			T obj = response.Result;
			obj.Id = Server.Db.Set<T>().First(c => c.Handle == obj.Handle).Id;

			player
				.Event($"igi:{typeof(T).Name}:claim")
				.Attach(obj)
				.Trigger();
		}

		public static async void ClaimObject<T>([FromSource] Player player, string json) where T : class, IObject
		{
			var response = RpcResponse<Guid>.Parse(json);

			var claimerSteamId = player.Identifiers["steam"];
			User claimerUser = Server.Db.Users.First(u => u.SteamId == claimerSteamId);

			T obj = Server.Db.Set<T>().First(c => c.Id == response.Result);

			Guid currentTrackerId = obj.TrackingUserId;
			obj.TrackingUserId = claimerUser.Id;

			Server.Db.Set<T>().AddOrUpdate(obj);
			await Server.Db.SaveChangesAsync();

			if (currentTrackerId == Guid.Empty) return;

			User currentTrackerUser = Server.Db.Users.First(u => u.Id == currentTrackerId);
			Player currentTrackerPlayer = Server.Instance.Players.First(p => p.Identifiers["steam"] == currentTrackerUser.SteamId);

			currentTrackerPlayer
				.Event($"igi:{typeof(T).Name}:unclaim")
				.Attach(obj)
				.Trigger();
		}

		public static async void UnclaimObject<T>([FromSource] Player player, string json) where T : class, IObject
		{
			var response = RpcResponse<int>.Parse(json);

			int netId = response.Result;
			T obj = Server.Db.Set<T>().FirstOrDefault(c => c.NetId == netId);
			if (obj == null) return;
			obj.TrackingUserId = Guid.Empty;
			obj.Handle = null;
			obj.NetId = null;

			Server.Db.Set<T>().AddOrUpdate(obj);
			await Server.Db.SaveChangesAsync();
		}
	}
}
