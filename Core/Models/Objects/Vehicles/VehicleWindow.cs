using System;
using CitizenFX.Core.Native;
using IgiCore.Core.Extensions;

namespace IgiCore.Core.Models.Objects.Vehicles
{
	public class VehicleWindow
	{
		public Guid Id { get; set; }
		public VehicleWindowIndex Index { get; set; }
		public bool IsIntact { get; set; }
		public bool IsRolledDown { get; set; }

		public VehicleWindow() { this.Id = GuidGenerator.GenerateTimeBasedGuid(); }
	}
}
