using System;
using IgiCore.Core.Extensions;

namespace IgiCore.Core.Models.Objects.Vehicles
{
	public class VehicleMod
	{
		public Guid Id { get; set; }
		public int Index { get; set; }
		public string Name { get; set; }
		public string TypeName { get; set; }
		public int Count { get; set; }
		public VehicleModType Type { get; set; }

		public VehicleMod()
		{
			this.Id = GuidGenerator.GenerateTimeBasedGuid();
		}
	}
}
