using System.Collections.Generic;
using CitizenFX.Core;
using Citizen = CitizenFX.Core.Player;
using System.Linq;
using System;
using System.Data.Entity;
using static IgiCore.Server.Server;
using IgiCore.Core.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IgiCore.Core.Extensions;

namespace IgiCore.Server.Models
{
    public class User : IUser
    {
        [Key]
        public Guid Id { get; set; }
        public string SteamId { get; set; }
        public string Name { get; set; }
        public virtual List<Character> Characters { get; set; }

        public User()
        {
            Id = GuidGenerator.GenerateTimeBasedGuid();
        }

        static public User GetOrCreate(Citizen citizen)
        {
            User user = null;

            DbContextTransaction transaction = Db.Database.BeginTransaction();
            string steamId = citizen.Identifiers["steam"];

            try
            {
                List<User> users = Db.Users.Where(u => u.SteamId == steamId).ToList();
                if (!users.Any())
                {
                    Debug.WriteLine($"User not found, creating new user for steamid: {citizen.Identifiers["steam"].ToString()}  with name: {citizen.Name}");
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
