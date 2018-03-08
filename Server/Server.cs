using System;
using System.Collections.Generic;
using CitizenFX.Core;
using Citizen = CitizenFX.Core.Player;
using System.Linq;
using static CitizenFX.Core.Native.API;
using IgiCore.Server.Storage.MySql;
using IgiCore.Server.Models;
using Newtonsoft.Json;

namespace IgiCore.Server
{
    public static class Config
    {
        public static string MySqlConnString => GetConvar("mysql_connection", string.Empty);
    }
    public partial class Server : BaseScript
    {

        public static DB Db;

        public Server()
        {
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

            EventHandlers["igi:character:save"] += new Action<string>(Character.Save);

            // Create database if not exists
            Db = new DB();
            Db.Database.CreateIfNotExists();
        }

        private Character NewCharCommand(Citizen citizen, string charName)
        {
            User user = User.GetOrCreate(citizen);

            Character character = new Character { UserId = user.Id, Name = charName };
            Db.Characters.Add(character);
            Db.SaveChanges();
            return character;
        }

        private Character GetCharCommand(Citizen citizen, Guid charId)
        {
            Character character = null;

            User user = User.GetOrCreate(citizen);

            character = user.Characters.Where(c => c.Id == charId).FirstOrDefault();

            return character;
        }

        private void OnPlayerSave([FromSource]Citizen citizen, string playerJson)
        {

        }

    }

}