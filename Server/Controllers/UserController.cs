using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using IgiCore.Core.Extensions;
using IgiCore.Models.Player;
using IgiCore.SDK.Core.Rpc;
using IgiCore.Server.Rpc;

namespace IgiCore.Server.Controllers
{
	public static class UserController
	{
		public static async Task<User> GetOrCreate(CitizenFX.Core.Player player)
		{
			User user = null;
			
			DbContextTransaction transaction = Server.Db.Database.BeginTransaction();
			
			try
			{
				var steamId = player.Identifiers["steam"];
				
				var users = Server.Db.Users.Where(u => u.SteamId == steamId).ToList();
				
				if (!users.Any())
				{
					user = new User
					{
						Id = GuidGenerator.GenerateTimeBasedGuid(),
						SteamId = steamId,
						Name = player.Name,
						AcceptedRules = null
					};

					Server.Db.Users.Add(user);


					Debug.WriteLine(steamId.Length.ToString());
					try
					{
						await Server.Db.SaveChangesAsync();
						Debug.WriteLine("3");
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

					Debug.WriteLine("2");
				}
				else
				{
					user = users.First();
				}

				transaction.Commit();
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
				throw;
			}
			catch (Exception ex)
			{
				transaction.Rollback();

				Debug.Write(ex.Message);
			}

			return user;
		}

		public static async void AcceptRules([FromSource] Player player, string json)
		{
			var response = RpcResponse<DateTime>.Parse(json);

			var user = await GetOrCreate(player);

			user.AcceptedRules = response.Result;

			Server.Db.Users.AddOrUpdate(user);
			await Server.Db.SaveChangesAsync();
		}

		public static async void Load([FromSource] Player player)
		{
			player
				.Event(RpcEvents.GetUser)
				.Attach(await GetOrCreate(player))
				.Trigger();
		}
	}
}
