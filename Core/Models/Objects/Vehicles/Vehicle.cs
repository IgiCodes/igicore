using System;
using System.Collections.Generic;
using IgiCore.Core.Extensions;

namespace IgiCore.Core.Models.Objects.Vehicles
{
    public class Vehicle : IVehicle
    {
        public Guid Id { get; set; }
        public int Hash { get; set; }
        public bool IsHoldable { get; set; }
        public string VIN { get; set; }
        public float BodyHealth { get; set; }
        public float EngineHealth { get; set; }
        public float DirtLevel { get; set; }
        public float FuelLevel { get; set; }
        public float OilLevel { get; set; }
        public float PetrolTankHealth { get; set; }
        public float Speed { get; set; }
        public float SteeringAngle { get; set; }
        public float TowingCraneRaisedAmount { get; set; }
        public bool HasAlarm { get; set; }
        public bool IsAlaramed { get; set; }
        public bool HasLock { get; set; }
        public bool IsDriveable { get; set; }
        public bool IsEngineRunning { get; set; }
        public bool HasSeatbelts { get; set; }
        public bool IsHighBeamsOn { get; set; }
        public bool IsLightsOn { get; set; }
        public bool IsInteriorLightOn { get; set; }
        public bool IsSearchLightOn { get; set; }
        public bool IsTaxiLightOn { get; set; }
        public bool IsLeftIndicatorLightOn { get; set; }
        public bool IsRightIndicatorLightOn { get; set; }
        public bool IsFrontBumperBrokenOff { get; set; }
        public bool IsRearBumperBrokenOff { get; set; }
        public bool IsLeftHeadLightBroken { get; set; }
        public bool IsRightHeadLightBroken { get; set; }
        public bool IsRadioEnabled { get; set; }
        public bool IsRoofOpen { get; set; }
        public bool NeedsToBeHotwired { get; set; }
        public bool CanTiresBurst { get; set; }
        public VehicleColor PrimaryColor { get; set; }
        public VehicleColor SecondayColor { get; set; }
        public VehicleColor PearescentColor { get; set; }
        public VehicleColor DashboardColor { get; set; }
        public VehicleColor RimColor { get; set; }
        public VehicleColor NeonColor { get; set; }
        public VehicleColor TireSmokeColor { get; set; }
        public VehicleColor TrimColor { get; set; }
        public VehicleWindowTint WindowTint { get; set; }
        public VehicleLockStatus LockStatus { get; set; }
        public RadioStation RadioStation { get; set; }
        public VehicleClass Class { get; set; }
        public List<VehicleExtra> Extras { get; set; }
        public List<VehicleWindow> Windows { get; set; }
        public List<VehicleSeat> Seats { get; set; }
        public List<VehicleMod> Mods { get; set; }
        public List<VehicleDoor> Doors { get; set; }
        public List<VehicleWheel> Wheels { get; set; }

        public Vehicle()
        {
            Id = GuidGenerator.GenerateTimeBasedGuid();
        }
    }
}