using System;
using IgiCore.SDK.Core.Helpers;
using Roleplay.Core.Models.Player;

namespace Roleplay.Vehicles.Core.Models
{
    public class VehicleSeat
    {
        public Guid Id { get; set; }
        public VehicleSeatIndex Index { get; set; }
        public Character Character { get; set; }

        public VehicleSeat() { this.Id = GuidGenerator.GenerateTimeBasedGuid(); }
    }
}
