using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using IgiCore.Core.Extensions;
using IgiCore.Core.Models.Player;

namespace IgiCore.Server.Models.Player
{
	public class Session : ISession
	{
		public Guid Id { get; set; }
		public string IpAddress { get; set; }
		public DateTime Connected { get; set; }
		public DateTime? Disconnected { get; set; }
		public string DisconnectReason { get; set; }

		public virtual User User { get; set; }

		public Session()
		{
			this.Id = GuidGenerator.GenerateTimeBasedGuid();
			this.Connected = DateTime.UtcNow;
		}

		public static async Task<Session> Create(CitizenFX.Core.Player player, User user)
		{
			var session = new Session
			{
				User = user,
				IpAddress = player.EndPoint
			};

			Server.Db.Sessions.Add(session);
			await Server.Db.SaveChangesAsync();

			return session;
		}

		public static async Task<Session> End(User user, string disconnectMessage)
		{
			var session = Server.Db.Sessions.Where(s => s.User.Id == user.Id && s.Disconnected == null && s.DisconnectReason == null).OrderBy(s => s.Connected).FirstOrDefault();

			if (session == null)
			{
				Server.Log("ERROR: No session to end");
				return null;
			}

			session.Disconnected = DateTime.UtcNow;
			session.DisconnectReason = disconnectMessage;

			Server.Db.Sessions.AddOrUpdate(session);
			await Server.Db.SaveChangesAsync();

			return session;
		}
	}
}
