using IgiCore.Core.Models.Player;
using System;
using IgiCore.Core.Extensions;

namespace IgiCore.Core.Models.Objects.Vehicles
{
    public class VehicleSeat
    {
        public Guid Id { get; set; }
        public VehicleSeatIndex Index { get; set; }
        public ICharacter Character { get; set; }

        public VehicleSeat()
        {
            Id = GuidGenerator.GenerateTimeBasedGuid();
        }
    }
}