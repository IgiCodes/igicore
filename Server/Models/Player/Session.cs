using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using IgiCore.Core.Extensions;
using IgiCore.Core.Models.Player;
using Citizen = CitizenFX.Core.Player;

namespace IgiCore.Server.Models.Player
{
	public class Session : ISession
	{
		[Key] public Guid Id { get; set; }
		public virtual User User { get; set; }
		public string IpAddress { get; set; }
		public DateTime Connected { get; set; }
		public DateTime? Disconnected { get; set; }

		public Session()
		{
			this.Id = GuidGenerator.GenerateTimeBasedGuid();
		}

		public static async Task<Session> Create(Citizen citizen, User user)
		{
			var session = new Session
			{
				User = user,
				IpAddress = citizen.EndPoint,
				Connected = DateTime.UtcNow
			};

			Server.Db.Sessions.Add(session);
			await Server.Db.SaveChangesAsync();

			return session;
		}

		public static async Task<Session> End(User user)
		{
			var session = Server.Db.Sessions.Where(s => s.Disconnected == null && s.User.Id == user.Id).OrderBy(s => s.Connected).FirstOrDefault();

			if (session == null)
			{
				Server.Log("ERROR: No session to end");
				return null;
			}

			session.Disconnected = DateTime.UtcNow;

			Server.Db.Sessions.AddOrUpdate(session);
			await Server.Db.SaveChangesAsync();

			return session;
		}
	}
}
