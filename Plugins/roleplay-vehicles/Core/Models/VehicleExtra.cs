using System;
using IgiCore.SDK.Core.Helpers;

namespace Roleplay.Vehicles.Core.Models
{
    public class VehicleExtra
    {
        public Guid Id { get; set; }
		public int Index { get; set; }
        public bool IsOn { get; set; }

		public virtual Vehicle Vehicle { get; set; }
		public Guid VehicleId { get; set; }

        public VehicleExtra() { this.Id = GuidGenerator.GenerateTimeBasedGuid(); }
    }
}
