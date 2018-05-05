using System;
using IgiCore.Core.Extensions;

namespace IgiCore.Core.Models.Objects.Vehicles
{
    public class VehicleExtra
    {
        public Guid Id { get; set; }
        public int Index { get; set; }
        public bool IsOn { get; set; }

        public VehicleExtra() { this.Id = GuidGenerator.GenerateTimeBasedGuid(); }
    }
}
