using System;
using IgiCore.Core.Extensions;

namespace IgiCore.Core.Models.Objects.Vehicles
{
	public class VehicleWheel
	{
		public Guid Id { get; set; }
		public VehicleWheelType Type { get; set; }
		public int Index { get; set; }
		public bool IsBurst { get; set; }

		public VehicleWheel()
		{
			this.Id = GuidGenerator.GenerateTimeBasedGuid();
		}
	}
}
