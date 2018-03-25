using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Migrations;
using System.IO.Ports;
using System.Linq;
using CitizenFX.Core;
using Citizen = CitizenFX.Core.Player;
using CitizenFX.Core.Native;
using CitizenFX;
using IgiCore.Core.Models.Objects.Vehicles;
using IgiCore.Server.Models;
using Newtonsoft.Json;

namespace IgiCore.Server.Services
{
    public class VehicleService : ServerService
    {
        private const int VehicleLoadDistance = 500;

        private Server server;
 
        public VehicleService(Server server)
        {
            this.server = server;
            HandleEvent<Citizen, string>("igi:character:save", LoadNearbyVehicles);
            HandleEvent<Citizen, string, CallbackDelegate>("playerDropped", ReassignTrackedVehicles);
        }

        public override void Initialise()
        {
            ResetVehicleTracking();
        }

        private static void ResetVehicleTracking()
        {
            Server.Log("Resetting vehicles");
            foreach (Vehicle dbVehicle in Server.Db.Vehicles.ToArray())
            {
                dbVehicle.Handle = null;
                dbVehicle.NetId = null;
                dbVehicle.TrackingUserId = Guid.Empty;
                Server.Db.Vehicles.AddOrUpdate(dbVehicle);
            }
            Server.Db.SaveChanges();
        }

        private void ReassignTrackedVehicles([FromSource] Citizen disconnectedCitizen, string disconnectMessage, CallbackDelegate kickReason)
        {
            Server.Log("ReassignTrackedVehicles called");
            string disconnectedSteamId = disconnectedCitizen.Identifiers["steam"];
            Server.Log($"Disconnected user steam id: {disconnectedSteamId}");
            User disconnectedUser = Server.Db.Users.First(u => u.SteamId == disconnectedSteamId);
            Server.Log($"Reassigning tracked vehicles for disconnected player: {disconnectedUser.Name}");
            List<Vehicle> vehicles = Server.Db.Vehicles.Where(v => v.TrackingUserId == disconnectedUser.Id).ToList();

            if (!vehicles.Any()) return;
            Server.Log("Vehicles found, looking up online users");
            List<User> users = new List<User>();
            foreach (Player serverPlayer in this.server.Players)
            {
                string steamId = serverPlayer.Identifiers["steam"];
                if (steamId == disconnectedSteamId) continue;
                users.Add(Server.Db.Users.First(u => u.SteamId == steamId));
            }

            Dictionary<Vehicle, Tuple<User, Character>> assignedVehicles = new Dictionary<Vehicle, Tuple<User, Character>>();

            Server.Log("Looping through vehicles to assign players");
            foreach (Vehicle vehicle in vehicles)
            {
                foreach (User user in users)
                {
                    foreach (Character character in user.Characters)
                    {
                        if (!(Vector3.Distance(character.Position, vehicle.Position) < VehicleLoadDistance)) continue;
                        if (!assignedVehicles.ContainsKey(vehicle))
                        {
                            assignedVehicles.Add(vehicle, new Tuple<User, Character>(user, character));
                        }
                        else
                        {
                            if (Vector3.Distance(character.Position, vehicle.Position) <
                                Vector3.Distance(assignedVehicles[vehicle].Item2.Position, vehicle.Position))
                            {
                                assignedVehicles[vehicle] = new Tuple<User, Character>(user, character);
                            }
                        }
                    }
                    
                }
            }

            if (!assignedVehicles.Any())
            {
                Server.Log("No vehicles assigned to anyone.");
                Citizen hostClient = null;
                try
                {
                    hostClient = server.Players.First(p => p.Handle == API.GetHostId());
                }
                catch (Exception e)
                {
                    Server.Log(e.Message);
                }
                foreach (Vehicle vehicle in vehicles)
                {
                    if (hostClient != null) BaseScript.TriggerClientEvent(hostClient, "igi:entity:delete", vehicle.NetId, vehicle.Hash);
                    vehicle.NetId = null;
                    vehicle.Handle = null;
                    vehicle.TrackingUserId = Guid.Empty;
                    Server.Db.Vehicles.AddOrUpdate(vehicle);
                    Server.Db.SaveChanges();
                }
            }

            foreach (KeyValuePair<Vehicle, Tuple<User, Character>> assignedVehicle in assignedVehicles)
            {
                Citizen citizen = this.server.Players.First(c =>
                    c.Identifiers["steam"] ==
                    Server.Db.Users.First(u => u.Id == assignedVehicle.Value.Item1.Id).SteamId);

                AssignVehicle(assignedVehicle.Key, citizen);
            }
        }

        private static void AssignVehicle(IVehicle vehicle, Citizen citizen)
        {
            string className = vehicle.GetType().BaseType.IsSubclassOf(typeof(Vehicle))
                ? vehicle.GetType().BaseType.Name
                : vehicle.GetType().Name;
            Server.Log($"Assinging vehicle to {citizen.Name} via event: 'igi:{className}:claim'");
            BaseScript.TriggerClientEvent(citizen, $"igi:{className}:claim", JsonConvert.SerializeObject(vehicle));   
        }

        private void LoadNearbyVehicles([FromSource]Citizen citizen, string charJson)
        {
            Character character = JsonConvert.DeserializeObject<Character>(charJson);
            Server.Log(character.ToString());
            Server.Log($"Checking nearby vehicles to spawn to position {character.Position.ToString()}");
            foreach (Vehicle vehicle in Server.Db.Vehicles.Where(v => v.Handle == null).ToArray())
            {
                Server.Log($"Checking {vehicle.Id} with position {new Vector3(vehicle.PosX, vehicle.PosY, vehicle.PosZ)}");
                if (!(Vector3.Distance(new Vector3(vehicle.PosX, vehicle.PosY, vehicle.PosZ), character.Position) <
                      VehicleLoadDistance)) continue;

                string className = vehicle.GetType().BaseType.IsSubclassOf(typeof(Vehicle))
                    ? vehicle.GetType().BaseType.Name
                    : vehicle.GetType().Name;
                Server.Log($"Spawning vehicle to {citizen.Name} via event: 'igi:{className}:spawn'");
                BaseScript.TriggerClientEvent(citizen, $"igi:{className}:spawn", JsonConvert.SerializeObject(vehicle));
            }
        }
    }
}