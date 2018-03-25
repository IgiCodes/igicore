using System;
using IgiCore.Core.Models.Objects.Vehicles;

namespace IgiCore.Core.Extensions
{
	public static class VehicleExtensions
	{
		public static Type VehicleType(this Type type)
		{
			Type baseType = type.BaseType;

			return baseType != null && baseType.IsSubclassOf(typeof(Vehicle)) ? baseType : type;
		}

		public static Type VehicleType(this IVehicle vehicle)
		{
			Type baseType = vehicle.GetType().BaseType;

			return baseType != null && baseType.IsSubclassOf(typeof(Vehicle)) ? baseType : vehicle.GetType();
		}
	}
}
