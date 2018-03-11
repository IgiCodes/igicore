using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using CitizenFX.Core;
using IgiCore.Core.Extensions;
using IgiCore.Core.Models;
using Citizen = CitizenFX.Core.Player;
using static IgiCore.Server.Server;
using Newtonsoft.Json;

namespace IgiCore.Server.Models
{
    public class User : IUser
    {
        [Key] public Guid Id { get; set; }
        public string SteamId { get; set; }
        public string Name { get; set; }
        public virtual List<Character> Characters { get; set; }

        public User()
        {
            Id = GuidGenerator.GenerateTimeBasedGuid();
        }

        public static void Load([FromSource]Citizen citizen)
        {
            BaseScript.TriggerClientEvent(citizen, "igi:user:load", JsonConvert.SerializeObject(GetOrCreate(citizen)));
        }

        public static User GetOrCreate(Citizen citizen)
        {
            User user = null;

            DbContextTransaction transaction = Db.Database.BeginTransaction();
            var steamId = citizen.Identifiers["steam"];

            try
            {
                var users = Db.Users.Where(u => u.SteamId == steamId).ToList();

                if (!users.Any())
                {
                    Debug.WriteLine($"User not found, creating new user for steamid: {citizen.Identifiers["steam"]}  with name: {citizen.Name}");

                    user = new User { SteamId = citizen.Identifiers["steam"], Name = citizen.Name };
                    Db.Users.Add(user);
                    Db.SaveChanges();
                }
                else
                {
                    user = users.First();
                    Debug.WriteLine($"User found for steamid: {user.SteamId}  with name: {user.Name}  and ID: {user.Id}");
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
