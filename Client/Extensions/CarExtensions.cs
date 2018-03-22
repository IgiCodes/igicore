using System.Threading.Tasks;
using CitizenFX.Core;
using IgiCore.Core.Models.Objects.Vehicles;

namespace IgiCore.Client.Extensions
{
    public static class CarExtensions
    {
        public static async Task<CitizenFX.Core.Vehicle> ToCitizenVehicle(this Car car)
        {
            CitizenFX.Core.Vehicle vehicle = await World.CreateVehicle(new Model((CitizenFX.Core.VehicleHash)car.Hash), car.Position);

            vehicle.BodyHealth = car.BodyHealth;
            vehicle.EngineHealth = car.EngineHealth;
            vehicle.DirtLevel = car.DirtLevel;
            vehicle.FuelLevel = car.FuelLevel;
            vehicle.OilLevel = car.OilLevel;
            vehicle.PetrolTankHealth = car.PetrolTankHealth;
            vehicle.TowingCraneRaisedAmount = car.TowingCraneRaisedAmount;
            //veh.HasAlarm = carToSpawn.HasAlarm;
            vehicle.IsAlarmSet = car.IsAlaramed;
            //veh.HasLock = carToSpawn.HasLock;
            vehicle.IsDriveable = car.IsDriveable;
            vehicle.IsEngineRunning = car.IsEngineRunning;
            //veh.HasSeatbelts = carToSpawn.HasSeatbelts;
            vehicle.CanTiresBurst = car.CanTiresBurst;

            return vehicle;
        }
    }
}
