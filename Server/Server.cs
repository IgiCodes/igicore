using System;
using System.Linq;
using CitizenFX.Core;
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
            EventHandlers["onResourceStarting"] += new Action<string>(ResourceStarting);
            EventHandlers["onResourceStart"] += new Action<string>(ResourceStart);
            EventHandlers["onResourceStop"] += new Action<string>(ResourceStop);

            EventHandlers["playerConnecting"] += new Action<Citizen, string, CallbackDelegate>(OnPlayerConnecting);
            EventHandlers["playerDropped"] += new Action<Citizen, string, CallbackDelegate>(OnPlayerDropped);

            EventHandlers["chatMessage"] += new Action<int, string, string>(OnChatMessage);

            EventHandlers["igi:user:load"] += new Action<Citizen>(User.Load);

            EventHandlers["igi:character:save"] += new Action<string>(Character.Save);
            
        }

        private Character NewCharCommand(Citizen citizen, string charName)
        {
            User user = User.GetOrCreate(citizen);

            Character character = new Character { UserId = user.Id, Name = charName };

            Db.Characters.Add(character);
            Db.SaveChanges();

            return character;
        }

        private Character GetCharCommand(Citizen citizen, string name)
        {
            Server.Log("GetCharCommand called");
            User user = User.GetOrCreate(citizen);
            Server.Log("GetCharCommand returning");
            return user.Characters.FirstOrDefault(c => c.Name == name);
        }

        private Character GetCharCommand(Citizen citizen, Guid charId)
        {
            User user = User.GetOrCreate(citizen);

            return user.Characters.FirstOrDefault(c => c.Id == charId);
        }

        public static void Log(string text)
        {
            Debug.WriteLine($"{DateTime.UtcNow:G} [SERVER]: {text}");
        }
    }
}