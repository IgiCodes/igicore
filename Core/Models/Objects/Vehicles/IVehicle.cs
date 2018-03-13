using System;
using System.Collections.Generic;
using CitizenFX.Core;
using IgiCore.Core.Models.Player;

namespace IgiCore.Core.Models.Objects.Vehicles
{
    public interface IVehicle : IObject
    {
        Guid Id { get; set; }
        string VIN { get; set; }
        float BodyHealth { get; set; }
        float EngineHealth { get; set; }
        float DirtLevel { get; set; }
        float FuelLevel { get; set; }
        float OilLevel { get; set; }
        float PetrolTankHealth { get; set; }
        float Speed { get; set; }
        float SteeringAngle { get; set; }
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
        VehicleColor SecondayColor { get; set; }
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
        ITrailer Trailer { get; set; }
        IVehicle TowedVehicle { get; set; }
        Dictionary<int, bool> Extras { get; set; }
        Dictionary<VehicleSeat, ICharacter> Occupants { get; set; }
        Dictionary<VehicleModType, VehicleMod> Mods { get; set; }
        Dictionary<VehicleWindowIndex, VehicleWindow> Windows { get; set; }
        Dictionary<VehicleDoorIndex, VehicleDoor> Doors { get; set; }
        Dictionary<int, VehicleWheel> Wheels { get; set; }
    }
}