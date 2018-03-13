using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using CitizenFX.Core;
using IgiCore.Core.Extensions;
using IgiCore.Server.Models;
using IgiCore.Server.Storage.MySql;
using Citizen = CitizenFX.Core.Player;

namespace IgiCore.Server
{
    public partial class Server : BaseScript
    {
        public static DB Db;

        public Server()
        {
            Db = new DB();
            Db.Database.CreateIfNotExists();

            RegisterEvents();
        }

        private void RegisterEvents()
        {
            this.EventHandlers["onResourceStarting"] += new Action<string>(ResourceStarting);
            this.EventHandlers["onResourceStart"] += new Action<string>(ResourceStart);
            this.EventHandlers["onResourceStop"] += new Action<string>(ResourceStop);

            this.EventHandlers["playerConnecting"] += new Action<Citizen, string, CallbackDelegate>(OnPlayerConnecting);
            this.EventHandlers["playerDropped"] += new Action<Citizen, string, CallbackDelegate>(OnPlayerDropped);

            this.EventHandlers["chatMessage"] += new Action<int, string, string>(OnChatMessage);

            this.EventHandlers["igi:user:load"] += new Action<Citizen>(User.Load);

            this.EventHandlers["igi:character:save"] += new Action<string>(Character.Save);

        }

        private static Character NewCharCommand(Citizen citizen, string charName)
        {
            User user = User.GetOrCreate(citizen);
            if (user.Characters == null) user.Characters = new List<Character>();
            Character character = new Character
            {
                Name = charName,
                Style = new Core.Models.Appearance.Style { Id = GuidGenerator.GenerateTimeBasedGuid() }
            };

            user.Characters.Add(character);
            Db.Users.AddOrUpdate(user);
            Db.SaveChanges();

            return character;
        }

        private static Character GetCharCommand(Citizen citizen, string name) => User.GetOrCreate(citizen).Characters.FirstOrDefault(c => c.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

        private static Character GetCharCommand(Citizen citizen, Guid charId) => User.GetOrCreate(citizen).Characters.FirstOrDefault(c => c.Id == charId);


        public static void Log(string text)
        {
            Debug.WriteLine($"{DateTime.UtcNow:G} [SERVER]: {text}");
        }
    }
}