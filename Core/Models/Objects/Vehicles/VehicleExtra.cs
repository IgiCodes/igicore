using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IgiCore.Core.Extensions;

namespace IgiCore.Core.Models.Objects.Vehicles
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
