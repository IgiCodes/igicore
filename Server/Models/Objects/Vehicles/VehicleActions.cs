using IgiCore.Core.Models.Objects.Vehicles;
using Newtonsoft.Json;
using System;
using System.Data.Entity.Migrations;
using System.Linq;
using static IgiCore.Server.Server;

namespace IgiCore.Server.Models.Objects.Vehicles
{
	public static class VehicleActions
	{
		public static void Save<T>(string vehicleJson) where T : class, IVehicle // Has no ID
		{
			T vehicle = JsonConvert.DeserializeObject<T>(vehicleJson);

			Log($"Saving vehicle {vehicle.Id} {vehicle.Handle}");

			if (vehicle.Id == Guid.Empty) vehicle.Id = Db.Set<T>().FirstOrDefault(c => c.Handle == vehicle.Handle)?.Id ?? Guid.Empty;
			if (vehicle.Id == Guid.Empty) return;

			T dbCar = Db.Set<T>().First(c => c.Id == vehicle.Id);
			if (vehicle.TrackingUserId != dbCar.TrackingUserId && dbCar.TrackingUserId != Guid.Empty) return;

			Db.Set<T>().AddOrUpdate(vehicle);
			Db.SaveChanges();
		}
	}
}
