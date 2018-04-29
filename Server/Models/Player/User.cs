using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using CitizenFX.Core;
using IgiCore.Core.Extensions;
using IgiCore.Core.Models.Player;
using Newtonsoft.Json;
using Citizen = CitizenFX.Core.Player;

namespace IgiCore.Server.Models.Player
{
    public class User : IUser
    {
        public virtual List<Character> Characters { get; set; }

        [Key] public Guid Id { get; set; }

        [MaxLength(17)] [Index(IsUnique = true)]
        public string SteamId { get; set; }

        public string Name { get; set; }

        public User() { this.Id = GuidGenerator.GenerateTimeBasedGuid(); }

        public static void Load([FromSource] Citizen citizen) { BaseScript.TriggerClientEvent(citizen, "igi:user:load", JsonConvert.SerializeObject(GetOrCreate(citizen))); }

        public static User GetOrCreate(Citizen citizen)
        {
            User user = null;

            DbContextTransaction transaction = Server.Db.Database.BeginTransaction();
            var steamId = citizen.Identifiers["steam"];

            try
            {
                var users = Server.Db.Users.Where(u => u.SteamId == steamId).ToList();

                if (!users.Any())
                {
                    user = new User {SteamId = citizen.Identifiers["steam"], Name = citizen.Name};

                    Debug.WriteLine(
                        $"User not found, creating new user for steamid: {user.SteamId} with name: {user.Name}");

                    Server.Db.Users.Add(user);
                    Server.Db.SaveChanges();
                }
                else
                {
                    user = users.First();

                    Debug.WriteLine($"User found for steamid: {user.SteamId} with name: {user.Name} and ID: {user.Id}");
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
