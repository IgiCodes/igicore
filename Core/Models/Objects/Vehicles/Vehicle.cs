using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CitizenFX.Core;
using IgiCore.Core.Extensions;
using Newtonsoft.Json;

namespace IgiCore.Core.Models.Objects.Vehicles
{
    public abstract class Vehicle : IVehicle
    {
        public Guid Id { get; set; }
        [Required]
        public uint Hash { get; set; }
        public int Handle { get; set; }
        public bool IsHoldable { get; set; } = false;
        public string VIN { get; set; }
        public float BodyHealth { get; set; } = 1000;
        public float EngineHealth { get; set; } = 1000;
        public float DirtLevel { get; set; }
        public float FuelLevel { get; set; } = 1000;
        public float OilLevel { get; set; } = 1000;
        public float PetrolTankHealth { get; set; } = 1000;
        public float TowingCraneRaisedAmount { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }
        public bool HasAlarm { get; set; } = true;
        public bool IsAlaramed { get; set; } = false;
        public bool HasLock { get; set; } = true;
        public bool IsDriveable { get; set; } = true;
        public bool IsEngineRunning { get; set; } = true;
        public bool HasSeatbelts { get; set; } = true;
        public bool IsHighBeamsOn { get; set; } = false;
        public bool IsLightsOn { get; set; } = false;
        public bool IsInteriorLightOn { get; set; } = false;
        public bool IsSearchLightOn { get; set; } = false;
        public bool IsTaxiLightOn { get; set; } = false;
        public bool IsLeftIndicatorLightOn { get; set; } = false;
        public bool IsRightIndicatorLightOn { get; set; } = false;
        public bool IsFrontBumperBrokenOff { get; set; } = false;
        public bool IsRearBumperBrokenOff { get; set; } = false;
        public bool IsLeftHeadLightBroken { get; set; } = false;
        public bool IsRightHeadLightBroken { get; set; } = false;
        public bool IsRadioEnabled { get; set; } = false;
        public bool IsRoofOpen { get; set; } = false;
        public bool NeedsToBeHotwired { get; set; } = false;
        public bool CanTiresBurst { get; set; } = true;
        public VehicleColor PrimaryColor { get; set; } = new VehicleColor();
        public VehicleColor SecondayColor { get; set; } = new VehicleColor();
        public VehicleColor PearescentColor { get; set; } = new VehicleColor();
        public VehicleColor DashboardColor { get; set; } = new VehicleColor();
        public VehicleColor RimColor { get; set; } = new VehicleColor();
        public VehicleColor NeonColor { get; set; } = new VehicleColor();
        public VehicleColor TireSmokeColor { get; set; } = new VehicleColor();
        public VehicleColor TrimColor { get; set; } = new VehicleColor();
        public VehicleWindowTint WindowTint { get; set; } = VehicleWindowTint.None;
        public VehicleLockStatus LockStatus { get; set; } = VehicleLockStatus.None;
        public RadioStation RadioStation { get; set; } = RadioStation.RadioOff;
        public VehicleClass Class { get; set; }
        public virtual List<VehicleExtra> Extras { get; set; } = new List<VehicleExtra>();
        public virtual List<VehicleWindow> Windows { get; set; } = new List<VehicleWindow>();
        public virtual List<VehicleSeat> Seats { get; set; } = new List<VehicleSeat>();
        public virtual List<VehicleMod> Mods { get; set; } = new List<VehicleMod>();
        public virtual List<VehicleDoor> Doors { get; set; } = new List<VehicleDoor>();
        public virtual List<VehicleWheel> Wheels { get; set; } = new List<VehicleWheel>();


        [JsonIgnore]
        public Vector3 Position
        {
            get => new Vector3(this.PosX, this.PosY, this.PosZ);
            set
            {
                this.PosX = value.X;
                this.PosY = value.Y;
                this.PosZ = value.Z;
            }
        }

        public Vehicle()
        {
            this.Id = GuidGenerator.GenerateTimeBasedGuid();
        }
    }
}