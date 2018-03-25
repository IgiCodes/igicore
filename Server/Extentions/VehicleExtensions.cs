using IgiCore.Core.Models.Objects.Vehicles;
using System;

namespace IgiCore.Server.Extentions
{
	public static class VehicleExtensions
	{
		public static Type VehicleType(this IVehicle vehicle)
		{
			Type baseType = vehicle.GetType().BaseType;

			return baseType != null && baseType.IsSubclassOf(typeof(Vehicle)) ? baseType : vehicle.GetType();
		}
	}
}
