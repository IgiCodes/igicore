using System;
using IgiCore.Core.Extensions;

namespace IgiCore.Core.Models.Objects.Vehicles
{
    public class VehicleDoor
    {
        public Guid Id { get; set; }
        public VehicleDoorIndex Index { get; set; }
        public bool IsOpen { get; set; } = false;
        public bool IsClosed => !this.IsOpen;
        public bool IsBroken { get; set; } = false;

        public VehicleDoor() { this.Id = GuidGenerator.GenerateTimeBasedGuid(); }
    }
}
