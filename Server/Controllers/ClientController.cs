using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using IgiCore.Models.Player;
using IgiCore.SDK.Core;
using IgiCore.SDK.Core.Diagnostics;
using IgiCore.SDK.Core.Exceptions;
using IgiCore.SDK.Server;
using IgiCore.Server.Events;
using IgiCore.Server.Storage;

namespace IgiCore.Server.Controllers
{
	public class ClientController : Controller
	{
		public ClientController(ILogger logger, EventsManager events) : base(logger, events)
		{
			API.EnableEnhancedHostSupport(true);

			//events.Event("playerConnecting").On(Connecting);
			//events.Event("playerDropped").On(Dropped);

			events.Event("test").On<bool>(Test);
		}

		public void Test(Client client, bool b)
		{
			this.Logger.Log($"Test: {b}");

			//this.Events.Event("test").Trigger(null, b);
		}

		public void Connecting([FromSource] Player player, string playerName, CallbackDelegate drop)
		{
			this.Logger.Log($"Connecting: {player.Name}");
		}

		public void Dropped([FromSource] Player player, string disconnectMessage, CallbackDelegate drop)
		{
			this.Logger.Log($"Dropped: {player.Name}");
		}

		public async Task<Session> Create(Player player, User user)
		{
			var session = new Session
			{
				Id = GuidGenerator.GenerateTimeBasedGuid(),
				User = user,
				IpAddress = player.EndPoint,
				Connected = DateTime.UtcNow
			};

			using (var entities = new StorageContext())
			{
				entities.Sessions.Add(session);

				await entities.SaveChangesAsync();
			}

			return session;
		}

		public async Task<Session> End(User user, string disconnectMessage)
		{
			using (var entities = new StorageContext())
			{
				var session = await entities.Sessions.OrderBy(s => s.Connected).SingleOrDefaultAsync(s => s.User.Id == user.Id && s.Disconnected == null && s.DisconnectReason == null);

				if (session == null)
				{
					throw new Exception($"No session to end for disconnected user \"{user.Id}\""); // TODO: SessionException
				}

				session.Disconnected = DateTime.UtcNow;
				session.DisconnectReason = disconnectMessage;

				await entities.SaveChangesAsync();

				return session;
			}
		}
	}
}
