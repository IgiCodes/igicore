using System;
using IgiCore.SDK.Core.Helpers;

namespace Roleplay.Vehicles.Core.Models
{
    public class VehicleWheel
    {
        public Guid Id { get; set; }
		public VehicleWheelType Type { get; set; }
		/// <summary>
		/// Gets or sets the Position.
		/// </summary>
		/// <value>
		/// Wheel index in VehicleWheelCollection
		/// </value>
		public VehicleWheelPosition Position { get; set; }
		/// <summary>
		/// Gets or sets the Index.
		/// </summary>
		/// <value>
		/// Wheel index from the type set.
		/// </value>
		public int Index { get; set; }
        public bool IsBurst { get; set; }

        public VehicleWheel() { this.Id = GuidGenerator.GenerateTimeBasedGuid(); }
    }
}
