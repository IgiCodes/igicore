using System.Threading.Tasks;
using CitizenFX.Core;

namespace IgiCore.Client.Extensions
{
	public static class VehicleExtensions
	{
		public static async Task<Vehicle> ToCitizenVehicle(this Core.Models.Objects.Vehicles.Vehicle vehicle)
		{
			Vehicle citizenVehicle = await World.CreateVehicle(new Model((VehicleHash)vehicle.Hash), vehicle.Position, vehicle.Heading);

			citizenVehicle.BodyHealth = vehicle.BodyHealth;
			citizenVehicle.EngineHealth = vehicle.EngineHealth;
			citizenVehicle.DirtLevel = vehicle.DirtLevel;
			citizenVehicle.FuelLevel = vehicle.FuelLevel;
			citizenVehicle.OilLevel = vehicle.OilLevel;
			citizenVehicle.PetrolTankHealth = vehicle.PetrolTankHealth;
			citizenVehicle.TowingCraneRaisedAmount = vehicle.TowingCraneRaisedAmount;
			//citizenVehicle.HasAlarm = vehicle.HasAlarm;
			citizenVehicle.IsAlarmSet = vehicle.IsAlaramed;
			//citizenVehicle.HasLock = vehicle.HasLock;
			citizenVehicle.IsDriveable = vehicle.IsDriveable;
			citizenVehicle.IsEngineRunning = vehicle.IsEngineRunning;
			//citizenVehicle.HasSeatbelts = vehicle.HasSeatbelts;
			citizenVehicle.AreHighBeamsOn = vehicle.IsHighBeamsOn;
			citizenVehicle.AreLightsOn = vehicle.IsLightsOn;
			citizenVehicle.IsInteriorLightOn = vehicle.IsInteriorLightOn;
			citizenVehicle.IsSearchLightOn = vehicle.IsSearchLightOn;
			citizenVehicle.IsTaxiLightOn = vehicle.IsTaxiLightOn;
			citizenVehicle.IsLeftIndicatorLightOn = vehicle.IsLeftIndicatorLightOn;
			citizenVehicle.IsRightIndicatorLightOn = vehicle.IsRightIndicatorLightOn;
			//citizenVehicle.IsFrontBumperBrokenOff = vehicle.IsFrontBumperBrokenOff;
			//citizenVehicle.IsRearBumperBrokenOff = vehicle.IsRearBumperBrokenOff;
			citizenVehicle.IsLeftHeadLightBroken = vehicle.IsLeftHeadLightBroken;
			citizenVehicle.IsRightHeadLightBroken = vehicle.IsRightHeadLightBroken;
			citizenVehicle.IsRadioEnabled = vehicle.IsRadioEnabled;
			citizenVehicle.RoofState = vehicle.IsRoofOpen ? VehicleRoofState.Opened : VehicleRoofState.Closed;
			citizenVehicle.NeedsToBeHotwired = vehicle.NeedsToBeHotwired;
			citizenVehicle.CanTiresBurst = vehicle.CanTiresBurst;

			if (vehicle.PrimaryColor.StockColor.HasValue)
			{
				citizenVehicle.Mods.PrimaryColor = (VehicleColor)(int)vehicle.PrimaryColor.StockColor.Value;
			}
			else
			{
				if (vehicle.PrimaryColor.CustomColor != null) citizenVehicle.Mods.CustomPrimaryColor = vehicle.PrimaryColor.CustomColor.Value;
			}

			if (vehicle.SecondaryColor.StockColor.HasValue)
			{
				citizenVehicle.Mods.SecondaryColor = (VehicleColor)(int)vehicle.SecondaryColor.StockColor.Value;
			}
			else
			{
				if (vehicle.SecondaryColor.CustomColor != null) citizenVehicle.Mods.CustomPrimaryColor = vehicle.SecondaryColor.CustomColor.Value;
			}

			citizenVehicle.Mods.WindowTint = (VehicleWindowTint)(int)vehicle.WindowTint;
			citizenVehicle.LockStatus = (VehicleLockStatus)(int)vehicle.LockStatus;
			citizenVehicle.RadioStation = (RadioStation)(int)vehicle.RadioStation;
			//citizenVehicle.ClassType = (CitizenFX.Core.VehicleClass)(int)vehicle.Class;
			citizenVehicle.Heading = vehicle.Heading;

			return citizenVehicle;
		}
	}
}
