using System.Collections.Generic;
using IgiCore.Models.Audio;

namespace IgiCore.Core.Models.Objects.Vehicles
{
    public interface IVehicle : IObject
    {
        string VIN { get; set; }
	    string LicensePlate { get; set; }
		float BodyHealth { get; set; }
        float EngineHealth { get; set; }
        float DirtLevel { get; set; }
        float FuelLevel { get; set; }
        float OilLevel { get; set; }
        float PetrolTankHealth { get; set; }
        float TowingCraneRaisedAmount { get; set; }
        bool HasAlarm { get; set; }
        bool IsAlaramed { get; set; }
        bool HasLock { get; set; }
        bool IsDriveable { get; set; }
        bool IsEngineRunning { get; set; }
        bool HasSeatbelts { get; set; }
        bool IsHighBeamsOn { get; set; }
        bool IsLightsOn { get; set; }
        bool IsInteriorLightOn { get; set; }
        bool IsSearchLightOn { get; set; }
        bool IsTaxiLightOn { get; set; }
        bool IsLeftIndicatorLightOn { get; set; }
        bool IsRightIndicatorLightOn { get; set; }
        bool IsFrontBumperBrokenOff { get; set; }
        bool IsRearBumperBrokenOff { get; set; }
        bool IsLeftHeadLightBroken { get; set; }
        bool IsRightHeadLightBroken { get; set; }
        bool IsRadioEnabled { get; set; }
        bool IsRoofOpen { get; set; }
        bool NeedsToBeHotwired { get; set; }
        bool CanTiresBurst { get; set; }
        VehicleColor PrimaryColor { get; set; }
        VehicleColor SecondaryColor { get; set; }
        VehicleColor PearescentColor { get; set; }
        VehicleColor DashboardColor { get; set; }
        VehicleColor RimColor { get; set; }
        VehicleColor NeonColor { get; set; }
        VehicleColor TireSmokeColor { get; set; }
        VehicleColor TrimColor { get; set; }
        VehicleWindowTint WindowTint { get; set; }
        VehicleLockStatus LockStatus { get; set; }
        RadioStation RadioStation { get; set; }
        VehicleClass Class { get; set; }
        List<VehicleExtra> Extras { get; set; }
        List<VehicleWindow> Windows { get; set; }
        List<VehicleSeat> Seats { get; set; }
        List<VehicleMod> Mods { get; set; }
        List<VehicleDoor> Doors { get; set; }
        List<VehicleWheel> Wheels { get; set; }
    }
}
