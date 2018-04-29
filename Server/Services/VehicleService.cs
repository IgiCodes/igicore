using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using IgiCore.Core.Extensions;
using IgiCore.Core.Models.Objects.Vehicles;
using IgiCore.Server.Models;
using IgiCore.Server.Models.Player;
using Newtonsoft.Json;
using Citizen = CitizenFX.Core.Player;
using static IgiCore.Server.Server;

namespace IgiCore.Server.Services
{
    public class VehicleService : ServerService
    {
        private const int VehicleLoadDistance = 500;

        private readonly Server server;

        public VehicleService(Server server)
        {
            this.server = server;

            HandleEvent<Citizen, string>("igi:character:save", LoadNearbyVehicles);
            HandleEvent<Citizen, string, CallbackDelegate>("playerDropped", ReassignTrackedVehicles);
        }

        public override void Initialise() { ResetVehicleTracking(); }

        private static void ResetVehicleTracking()
        {
            Log("Resetting vehicles");

            foreach (Vehicle dbVehicle in Db.Vehicles.ToArray())
            {
                dbVehicle.Handle = null;
                dbVehicle.NetId = null;
                dbVehicle.TrackingUserId = Guid.Empty;

                Db.Vehicles.AddOrUpdate(dbVehicle);
            }

            Db.SaveChanges();
        }

        private void ReassignTrackedVehicles(
            [FromSource] Citizen disconnectedCitizen, string disconnectMessage,
            CallbackDelegate kickReason)
        {
            Log("ReassignTrackedVehicles called");

            var disconnectedSteamId = disconnectedCitizen.Identifiers["steam"];
            Log($"Disconnected user steam id: {disconnectedSteamId}");

            User disconnectedUser = Db.Users.First(u => u.SteamId == disconnectedSteamId);
            Log($"Reassigning tracked vehicles for disconnected player: {disconnectedUser.Name}");

            var vehicles = Db.Vehicles.Where(v => v.TrackingUserId == disconnectedUser.Id).ToList();
            if (!vehicles.Any()) return;

            Log("Vehicles found, looking up online users");

            var users = new List<User>();
            foreach (Citizen serverPlayer in this.server.Players)
            {
                var steamId = serverPlayer.Identifiers["steam"];
                if (steamId == disconnectedSteamId) continue;

                users.Add(Db.Users.First(u => u.SteamId == steamId));
            }

            Log("Looping through vehicles to assign players");

            var assignedVehicles = new Dictionary<Vehicle, Tuple<User, Character>>();
            foreach (Vehicle vehicle in vehicles)
            foreach (User user in users)
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
                        Vector3.Distance(assignedVehicles[vehicle].Item2.Position, vehicle.Position)) assignedVehicles[vehicle] = new Tuple<User, Character>(user, character);
                }
            }

            if (!assignedVehicles.Any())
            {
                Log("No vehicles assigned to anyone");

                Citizen hostClient = null;
                try { hostClient = this.server.Players.First(p => p.Handle == API.GetHostId()); }
                catch (Exception ex) { Log(ex.Message); }

                foreach (Vehicle vehicle in vehicles)
                {
                    if (hostClient != null) BaseScript.TriggerClientEvent(hostClient, "igi:entity:delete", vehicle.NetId, vehicle.Hash);

                    vehicle.NetId = null;
                    vehicle.Handle = null;
                    vehicle.TrackingUserId = Guid.Empty;

                    Db.Vehicles.AddOrUpdate(vehicle);
                    Db.SaveChanges();
                }
            }

            foreach (var assignedVehicle in assignedVehicles)
            {
                Citizen citizen = this.server.Players.First(
                    c =>
                        c.Identifiers["steam"] == Db.Users.First(u => u.Id == assignedVehicle.Value.Item1.Id).SteamId);

                AssignVehicle(assignedVehicle.Key, citizen);
            }
        }

        private static void AssignVehicle(IVehicle vehicle, Citizen citizen)
        {
            Log($"Assinging vehicle to {citizen.Name} via event: 'igi:{vehicle.VehicleType().Name}:claim'");

            BaseScript.TriggerClientEvent(
                citizen,
                $"igi:{vehicle.VehicleType().Name}:claim",
                JsonConvert.SerializeObject(vehicle));
        }

        private static void LoadNearbyVehicles([FromSource] Citizen citizen, string charJson)
        {
            Character character = JsonConvert.DeserializeObject<Character>(charJson);

            Log($"Checking nearby vehicles to spawn to position {character.Position.ToString()}");

            foreach (Vehicle vehicle in Db.Vehicles.Where(v => v.Handle == null).ToArray())
            {
                Log($"Checking {vehicle.Id} with position {vehicle.Position}");
                if (!(Vector3.Distance(vehicle.Position, character.Position) < VehicleLoadDistance)) continue;

                Log($"Spawning vehicle to {citizen.Name} via event: 'igi:{vehicle.VehicleType().Name}:spawn'");
                BaseScript.TriggerClientEvent(
                    citizen,
                    $"igi:{vehicle.VehicleType().Name}:spawn",
                    JsonConvert.SerializeObject(vehicle));
            }
        }
    }
}
