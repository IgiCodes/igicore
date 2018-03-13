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
                    Log("/newchar command called");

                    TriggerClientEvent(citizen, "igi:character:new", JsonConvert.SerializeObject(NewCharCommand(citizen, args[0])));

                    break;
                case "/char":
                    Log("/char command called");

                    TriggerClientEvent(citizen, "igi:character:load", JsonConvert.SerializeObject(GetCharCommand(citizen, args[0])));

                    break;
                case "/gps":
                    Log("/gps command called");

                    TriggerClientEvent(citizen, "igi:user:gps");

                    break;
                case "/component":
                    Log("/component command called");

                    TriggerClientEvent(citizen, "igi:character:component:set", int.Parse(args[0]), int.Parse(args[1]), int.Parse(args[2]));

                    break;
                case "/prop":
                    Log("/prop command called");

                    TriggerClientEvent(citizen, "igi:character:prop:set", int.Parse(args[0]), int.Parse(args[1]), int.Parse(args[2]));

                    break;
                case "/car":
                    Log("/car command called");

                    TriggerClientEvent(citizen, "igi:vehicle:spawn", args[0]);

                    break;
                default:
                    Log("Unknown command");

                    break;
            }
        }
    }
}
