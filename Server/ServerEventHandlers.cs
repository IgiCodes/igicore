using System.Collections.Generic;
using System.Linq;
using CitizenFX.Core;
using IgiCore.Server.Models;
using Newtonsoft.Json;
using Citizen = CitizenFX.Core.Player;

namespace IgiCore.Server
{
    public partial class Server
    {
        private void ResourceStarting(string resourceName)
        {
            //Debug.WriteLine($"Starting resource: {resourceName}");
        }

        private void ResourceStart(string resourceName)
        {
            //Debug.WriteLine($"Start resource: {resourceName}");
        }

        private void ResourceStop(string resourceName)
        {
            //Debug.WriteLine($"Stopping resource: {resourceName}");
        }

        private void OnPlayerConnecting([FromSource] Citizen citizen, string playerName, CallbackDelegate kickReason)
        {
            User.GetOrCreate(citizen);
        }

        private void OnPlayerDropped([FromSource] Citizen citizen, string disconnectMessage, CallbackDelegate kickReason)
        {
            //Debug.WriteLine($"Disconnected: {citizen.Name}");
        }

        private void OnChatMessage(int playerId, string playerName, string message)
        {
            //Debug.WriteLine($"New Chat Message from: {playerId}...");
            //Debug.WriteLine($"Player: {playerName}");
            //Debug.WriteLine($"Message: {message}");
            
            Citizen citizen = Players[playerId];

            List<string> args = message.Split(' ').ToList();
            string command = args.First();
            args = args.Skip(1).ToList();

            switch (command)
            {
                case "/newchar":
                    Debug.WriteLine("[SERVER]: /newchar command called");

                    TriggerClientEvent(citizen, "igi:character:new", JsonConvert.SerializeObject(NewCharCommand(citizen, args[0])));

                    break;
                case "/char":
                    Debug.WriteLine("[SERVER]: /char command called");
                    
                    TriggerClientEvent(citizen, "igi:character:load", JsonConvert.SerializeObject(GetCharCommand(citizen, args[0])));

                    break;
                case "/gps":
                    Debug.WriteLine("[SERVER]: /gps command called");

                    TriggerClientEvent(citizen, "igi:user:gps");

                    break;
                default:
                    Debug.WriteLine("[SERVER]: Unknown command");

                    break;
            }
        }
    }
}
