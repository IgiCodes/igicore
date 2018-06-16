using System;
using IgiCore.SDK.Core.Helpers;

namespace Roleplay.Vehicles.Core.Models
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
