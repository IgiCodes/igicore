using System.Threading.Tasks;
using CitizenFX.Core;
using Vehicle = IgiCore.Core.Models.Objects.Vehicles.Vehicle;

namespace IgiCore.Client.Extensions
{
	public static class VehicleExtensions
	{
		public static async Task<CitizenFX.Core.Vehicle> ToCitizenVehicle(this Vehicle vehicle)
		{
			CitizenFX.Core.Vehicle citizenVehicle = await World.CreateVehicle(new Model((VehicleHash)vehicle.Hash), vehicle.Position, vehicle.Heading);

			citizenVehicle.BodyHealth = vehicle.BodyHealth;
			citizenVehicle.EngineHealth = vehicle.EngineHealth;
			citizenVehicle.DirtLevel = vehicle.DirtLevel;
			citizenVehicle.FuelLevel = vehicle.FuelLevel;
			citizenVehicle.OilLevel = vehicle.OilLevel;
			citizenVehicle.PetrolTankHealth = vehicle.PetrolTankHealth;
			citizenVehicle.TowingCraneRaisedAmount = vehicle.TowingCraneRaisedAmount;
			//veh.HasAlarm = carToSpawn.HasAlarm;
			citizenVehicle.IsAlarmSet = vehicle.IsAlaramed;
			//veh.HasLock = carToSpawn.HasLock;
			citizenVehicle.IsDriveable = vehicle.IsDriveable;
			citizenVehicle.IsEngineRunning = vehicle.IsEngineRunning;
			//veh.HasSeatbelts = carToSpawn.HasSeatbelts;
			citizenVehicle.CanTiresBurst = vehicle.CanTiresBurst;

			return citizenVehicle;
		}
	}
}
