using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using CitizenFX.Core;
using IgiCore.Models.Player;
using IgiCore.SDK.Core.Diagnostics;
using IgiCore.SDK.Core.Helpers;
using IgiCore.SDK.Server.Controllers;
using IgiCore.SDK.Server.Events;
using IgiCore.SDK.Server.Rpc;
using IgiCore.Server.Rpc;
using IgiCore.Server.Storage;

namespace IgiCore.Server.Controllers
{
	public class ClientController : Controller
	{
		public ClientController(ILogger logger, IEventManager events, IRpcHandler rpc) : base(logger, events, rpc)
		{
			this.Rpc.Event("playerConnecting").OnRaw(new Action<Player, string, CallbackDelegate>(Connecting));
			this.Rpc.Event("playerDropped").OnRaw(new Action<Player, string, CallbackDelegate>(Dropped));
			this.Rpc.Event("ready").On<string>(Ready);
		}

		public async void Connecting([FromSource] Player player, string playerName, CallbackDelegate drop)
		{
			var client = new Client(int.Parse(player.Handle));

			await this.Events.RaiseAsync("clientConnecting", client);

			using (var context = new StorageContext())
			using (var transaction = context.Database.BeginTransaction())
			{
				try
				{
					var user = context.Users.SingleOrDefault(u => u.SteamId == client.SteamId); // TODO: Async crashes?!?

					if (user == default(User))
					{
						// Create user
						user = new User
						{
							Id = GuidGenerator.GenerateTimeBasedGuid(),
							SteamId = client.SteamId,
							Name = client.Name
						};

						context.Users.Add(user);
					}
					else
					{
						// Update name
						user.Name = client.Name;
					}
					
					// Create session
					var session = new Session
					{
						Id = GuidGenerator.GenerateTimeBasedGuid(),
						User = user,
						IpAddress = client.EndPoint
					};

					context.Sessions.Add(session);

					// Save changes
					await context.SaveChangesAsync();
					transaction.Commit();

					this.Logger.Info($"[{session.Id}] Player \"{user.Name}\" connected from {session.IpAddress}");
				}
				catch (Exception ex)
				{
					transaction.Rollback();

					this.Logger.Error(ex);
				}
			}
		}

		public async void Dropped([FromSource] Player player, string disconnectMessage, CallbackDelegate drop)
		{
			var client = new Client(int.Parse(player.Handle));
			
			using (var context = new StorageContext())
			{
				context.Configuration.LazyLoadingEnabled = false;

				var user = context.Users.SingleOrDefault(u => u.SteamId == client.SteamId);
				if (user == null) throw new Exception($"No user to end for disconnected client \"{client.SteamId}\""); // TODO: SessionException

				var session = context.Sessions.OrderBy(s => s.Connected).FirstOrDefault(s => s.User.Id == user.Id && s.Disconnected == null && s.DisconnectReason == null);
				if (session == null) throw new Exception($"No session to end for disconnected user \"{user.Id}\""); // TODO: SessionException

				session.Disconnected = DateTime.UtcNow;
				session.DisconnectReason = disconnectMessage;

				await context.SaveChangesAsync();

				this.Logger.Info($"[{session.Id}] Player \"{user.Name}\" disconnected: {session.DisconnectReason}");
			}
		}

		public void Ready(IRpcEvent e, string clientVersion)
		{
			this.Logger.Info($"Ready: {e.User.Name} {clientVersion}");

			e.Reply(e.User);
		}
	}
}
