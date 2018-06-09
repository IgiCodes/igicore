using System;
using System.Collections.Generic;
using System.Dynamic;
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
			this.Rpc.Event("playerConnecting").OnRaw(new Action<Player, string, CallbackDelegate, ExpandoObject>(Connecting));
			this.Rpc.Event("playerDropped").OnRaw(new Action<Player, string, CallbackDelegate>(Dropped));
			this.Rpc.Event("ready").On<string>(Ready);
			this.Rpc.Event("characters:list").On(Characters);
		}

		public async void Connecting([FromSource] Player player, string playerName, CallbackDelegate drop, ExpandoObject deferrals)
		{
			var client = new Client(int.Parse(player.Handle));

			await this.Events.RaiseAsync("clientConnecting", client); // TODO

			using (var context = new StorageContext())
			using (var transaction = context.Database.BeginTransaction())
			{
				context.Configuration.ProxyCreationEnabled = false;
				context.Configuration.LazyLoadingEnabled = false;

				try
				{
					var user = context.Users.SingleOrDefault(u => u.SteamId == client.SteamId);

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

		public void Characters(IRpcEvent e)
		{
			this.Logger.Info($"Characters: {e.User.Name}");

			using (var context = new StorageContext())
			{
				var characters = context.Characters.Where(c => c.User.Id == e.User.Id && c.Deleted == null).OrderBy(c => c.Created).ToList();

				e.Reply(characters);
			}
		}
	}
}
