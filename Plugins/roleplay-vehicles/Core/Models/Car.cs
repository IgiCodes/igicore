namespace Roleplay.Vehicles.Core.Models
{
    public class Car : Vehicle, ICar
    {
        public Trailer Trailer { get; set; }
        public Vehicle TowedVehicle { get; set; }

		public Car() { }

	    public Car(Vehicle vehicle)
	    {
			Id = vehicle.Id;
			Hash = vehicle.Hash;
			Handle = vehicle.Handle;
			TrackingUserId = vehicle.TrackingUserId;
			NetId = vehicle.NetId;
		    Position = vehicle.Position;
			VIN = vehicle.VIN;
			LicensePlate = vehicle.LicensePlate;
			BodyHealth = vehicle.BodyHealth;
			EngineHealth = vehicle.EngineHealth;
			DirtLevel = vehicle.DirtLevel;
			FuelLevel = vehicle.FuelLevel;
			OilLevel = vehicle.OilLevel;
			PetrolTankHealth = vehicle.PetrolTankHealth;
			TowingCraneRaisedAmount = vehicle.TowingCraneRaisedAmount;
			PosX = vehicle.PosX;
			PosY = vehicle.PosY;
			PosZ = vehicle.PosZ;
			Heading = vehicle.Heading;
			HasAlarm = vehicle.HasAlarm;
			IsAlarmed = vehicle.IsAlarmed;
			IsAlarmSounding = vehicle.IsAlarmSounding;
			HasLock = vehicle.HasLock;
			IsDriveable = vehicle.IsDriveable;
			IsEngineRunning = vehicle.IsEngineRunning;
			HasSeatbelts = vehicle.HasSeatbelts;
			IsHighBeamsOn = vehicle.IsHighBeamsOn;
			IsLightsOn = vehicle.IsLightsOn;
			IsInteriorLightOn = vehicle.IsInteriorLightOn;
			IsSearchLightOn = vehicle.IsSearchLightOn;
			IsTaxiLightOn = vehicle.IsTaxiLightOn;
			IsLeftIndicatorLightOn = vehicle.IsLeftIndicatorLightOn;
			IsRightIndicatorLightOn = vehicle.IsRightIndicatorLightOn;
			IsFrontBumperBrokenOff = vehicle.IsFrontBumperBrokenOff;
			IsRearBumperBrokenOff = vehicle.IsRearBumperBrokenOff;
			IsLeftHeadLightBroken = vehicle.IsLeftHeadLightBroken;
			IsRightHeadLightBroken = vehicle.IsRightHeadLightBroken;
			IsRadioEnabled = vehicle.IsRadioEnabled;
			IsRoofOpen = vehicle.IsRoofOpen;
			HasRoof = vehicle.HasRoof;
			IsVehicleConvertible = vehicle.IsVehicleConvertible;
			NeedsToBeHotwired = vehicle.NeedsToBeHotwired;
			CanTiresBurst = vehicle.CanTiresBurst;
			PrimaryColor = vehicle.PrimaryColor;
			SecondaryColor = vehicle.SecondaryColor;
			PearescentColor = vehicle.PearescentColor;
			DashboardColor = vehicle.DashboardColor;
			RimColor = vehicle.RimColor;
			NeonColor = vehicle.NeonColor;
			NeonPositions = vehicle.NeonPositions;
			TireSmokeColor = vehicle.TireSmokeColor;
			TrimColor = vehicle.TrimColor;
			WindowTint = vehicle.WindowTint;
			LockStatus = vehicle.LockStatus;
			RadioStation = vehicle.RadioStation;
			Class = vehicle.Class;
			Extras = vehicle.Extras;
			Windows = vehicle.Windows;
			Seats = vehicle.Seats;
			Mods = vehicle.Mods;
			Doors = vehicle.Doors;
			Wheels = vehicle.Wheels;
		}
	}
}
