using System.Collections.Generic;

namespace Roleplay.Vehicles.Core.Models
{
	public static class VehicleWheelBones
	{
		public static readonly Dictionary<VehicleWheelPosition, string> Bones = new Dictionary<VehicleWheelPosition, string>()
		{
			[VehicleWheelPosition.LeftFront] = "wheel_lf",
			[VehicleWheelPosition.RightFront] = "wheel_rf",
			[VehicleWheelPosition.LeftMiddle] = "wheel_lm1",
			[VehicleWheelPosition.RightMiddle] = "wheel_rm1",
			[VehicleWheelPosition.LeftRear] = "wheel_lr",
			[VehicleWheelPosition.RightRear] = "wheel_rr",
		};
	}
}
