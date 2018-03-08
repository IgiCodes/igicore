using CitizenFX.Core;
using Citizen = CitizenFX.Core.Player;
using static CitizenFX.Core.Native.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IgiCore.Server.Models;
using Newtonsoft.Json;
using IgiCore.Core.Models;

namespace IgiCore.Server
{
    public partial class Server
    {
        private void ResourceStarting(string resourceName)
        {
            Debug.WriteLine($"Starting resource: {resourceName}");
        }

        private void ResourceStart(string resourceName)
        {
            Debug.WriteLine($"Start resource: {resourceName}");
            if (GetCurrentResourceName() == resourceName)
            {
            }
        }

        private void ResourceStop(string resourceName)
        {
            Debug.WriteLine($"Stopping resource: {resourceName}");
            if (GetCurrentResourceName() == resourceName)
            {
            }
        }

        private void OnPlayerConnecting([FromSource]Citizen citizen, string playerName, CallbackDelegate kickReason)
        {
            User.GetOrCreate(citizen);
        }

        private void OnPlayerDropped([FromSource]Citizen citizen, string disconnectMessage, CallbackDelegate kickReason)
        {
            Debug.WriteLine($"Disconnected: {citizen.Name}");
        }

        private void OnChatMessage(int playerId, string playerName, string message)
        {
            //Debug.WriteLine($"New Chat Message from: {playerId}...");
            //Debug.WriteLine($"Player: {playerName}");
            //Debug.WriteLine($"Message: {message}");

            // Get the calling Citizen
            Citizen citizen = this.Players[playerId];
            // Parse the input
            List<string> args = message.Split(new[] { ' ' }).ToList();
            string command = args.First();
            args = args.Skip(1).ToList();

            //charCommand
            if (command == "/newchar")
            {
                Debug.WriteLine("newchar command called");
                string charName = args[0];
                Character character = NewCharCommand(citizen, charName);

                TriggerClientEvent(citizen, "igi:character:new", JsonConvert.SerializeObject(character));

                return;
            }
            
            if (command == "/char")
            {
                Guid charId = Guid.Parse(args[0]);
                Character character = GetCharCommand(citizen, charId);

                TriggerClientEvent(citizen, "igi:character:load", JsonConvert.SerializeObject(character));

                return;
            }

            if (command == "/gps")
            {
                Debug.WriteLine("gps command called");
                TriggerClientEvent(citizen, "igi:user:gps");

                return;
            }

        }
    }
}
