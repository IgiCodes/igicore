using System;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using IgiCore.Core.Extensions;
using IgiCore.Models.Player;

namespace IgiCore.Server.Controllers
{
	public class SessionController
	{
		public static async Task<Session> Create(CitizenFX.Core.Player player, User user)
		{
			var session = new Session
			{
				Id = GuidGenerator.GenerateTimeBasedGuid(),
				User = user,
				IpAddress = player.EndPoint,
				Connected = DateTime.UtcNow
			};

			Server.Db.Sessions.Add(session);

			try { 
				await Server.Db.SaveChangesAsync();
			}
			catch (DbEntityValidationException ex)
			{
				foreach (var eve in ex.EntityValidationErrors)
				{
					Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);

					foreach (var ve in eve.ValidationErrors)
					{
						Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
					}
				}

				//throw;
			}

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
			
			await Server.Db.SaveChangesAsync();

			return session;
		}
	}
}
