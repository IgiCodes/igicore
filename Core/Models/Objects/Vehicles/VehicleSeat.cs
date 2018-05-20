using System;
using IgiCore.Core.Extensions;
using IgiCore.Models.Player;

namespace IgiCore.Core.Models.Objects.Vehicles
{
    public class VehicleSeat
    {
        public Guid Id { get; set; }
        public VehicleSeatIndex Index { get; set; }
        public Character Character { get; set; }

        public VehicleSeat() { this.Id = GuidGenerator.GenerateTimeBasedGuid(); }
    }
}
