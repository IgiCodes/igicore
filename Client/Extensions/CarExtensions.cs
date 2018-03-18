using System.Threading.Tasks;
using CitizenFX.Core;
using IgiCore.Core.Models.Objects.Vehicles;

namespace IgiCore.Client.Extensions
{
    public static class CarExtensions
    {
        public static async Task<CitizenFX.Core.Vehicle> ToCitizenVehcle(this Car car)
        {
            CitizenFX.Core.Vehicle vehcle = await World.CreateVehicle(new Model((CitizenFX.Core.VehicleHash)car.Hash), car.Position);

            vehcle.BodyHealth = car.BodyHealth;
            vehcle.EngineHealth = car.EngineHealth;
            vehcle.DirtLevel = car.DirtLevel;
            vehcle.FuelLevel = car.FuelLevel;
            vehcle.OilLevel = car.OilLevel;
            vehcle.PetrolTankHealth = car.PetrolTankHealth;
            vehcle.TowingCraneRaisedAmount = car.TowingCraneRaisedAmount;
            //veh.HasAlarm = carToSpawn.HasAlarm;
            vehcle.IsAlarmSet = car.IsAlaramed;
            //veh.HasLock = carToSpawn.HasLock;
            vehcle.IsDriveable = car.IsDriveable;
            vehcle.IsEngineRunning = car.IsEngineRunning;
            //veh.HasSeatbelts = carToSpawn.HasSeatbelts;
            vehcle.CanTiresBurst = car.CanTiresBurst;

            return vehcle;
        }
    }
}
