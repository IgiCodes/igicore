using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using IgiCore.Core.Extensions;
using IgiCore.Core.Models.Player;
using Newtonsoft.Json;

namespace IgiCore.Server.Models.Player
{
	public class User : IUser
	{
		[Key] public Guid Id { get; set; }
		[MaxLength(17)]
		[Index(IsUnique = true)]
		public string SteamId { get; set; }
		public string Name { get; set; }
		public DateTime? AcceptedRules { get; set; }
		public DateTime Created { get; set; }

		public virtual List<Character> Characters { get; set; }

		public User()
		{
			this.Id = GuidGenerator.GenerateTimeBasedGuid();
			this.Created = DateTime.UtcNow;
		}

		public static async void Load([FromSource] CitizenFX.Core.Player player)
		{
			BaseScript.TriggerClientEvent(player, "igi:user:load", JsonConvert.SerializeObject(await GetOrCreate(player)));
		}

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
						SteamId = steamId,
						Name = player.Name,
						AcceptedRules = null
					};

					Server.Db.Users.Add(user);
					await Server.Db.SaveChangesAsync();
				}
				else
				{
					user = users.First();
				}

				transaction.Commit();
			}
			catch (Exception ex)
			{
				transaction.Rollback();

				Debug.Write(ex.Message);
			}

			return user;
		}
	}
}
