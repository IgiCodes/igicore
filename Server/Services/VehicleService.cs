using System;
using System.Data.Entity.Migrations;
using System.Linq;
using CitizenFX.Core;
using Citizen = CitizenFX.Core.Player;
using IgiCore.Core.Models.Objects.Vehicles;
using IgiCore.Server.Models;
using Newtonsoft.Json;

namespace IgiCore.Server.Services
{
    public class VehicleService : ServerService
    {
        public VehicleService()
        {
            HandleEvent<Citizen, string>("igi:character:save", LoadNearbyVehicles);
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
                Server.Db.Vehicles.AddOrUpdate(dbVehicle);
            }
            Server.Db.SaveChanges();
        }

        private void LoadNearbyVehicles([FromSource]Citizen citizen, string charJson)
        {
            Character character = JsonConvert.DeserializeObject<Character>(charJson);
            Server.Log(character.ToString());
            Server.Log($"Checking nearby vehicles to spawn to position {character.Position.ToString()}");
            foreach (Car car in Server.Db.Cars.Where(v => v.Handle == null).ToArray())
            {
                Server.Log($"Checking {car.Id} with position {car.Position.ToString()}");
                if (Vector3.Distance(car.Position, character.Position) < 500)
                {
                    Server.Log($"Spawning {car.Id}");
                    BaseScript.TriggerClientEvent(citizen, "igi:car:spawn", JsonConvert.SerializeObject(car));
                }
            }
        }
    }
}