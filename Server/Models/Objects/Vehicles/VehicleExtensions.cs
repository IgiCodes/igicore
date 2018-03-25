using System;
using CitizenFX.Core;
using IgiCore.Core.Models.Objects.Vehicles;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using IgiCore.Core.Models.Objects;
using Newtonsoft.Json;

namespace IgiCore.Server.Models.Objects.Vehicles
{
	public static class VehicleExtensions
	{
		public static void Save<T>(string vehicleJson) where T : class, IVehicle // Has no ID
		{
		    T vehicle = JsonConvert.DeserializeObject<T>(vehicleJson);
			Server.Log($"Saving vehicle {vehicle.Id} {vehicle.Handle}");

            if (vehicle.Id == Guid.Empty) vehicle.Id = Server.Db.Set<T>().FirstOrDefault(c => c.Handle == vehicle.Handle)?.Id ?? Guid.Empty;
		    if (vehicle.Id == Guid.Empty) return;
		    T dbCar = Server.Db.Set<T>().First(c => c.Id == vehicle.Id);

            if (vehicle.TrackingUserId != dbCar.TrackingUserId && dbCar.TrackingUserId != Guid.Empty) return;
            Server.Db.Set<T>().AddOrUpdate(vehicle);
			Server.Db.SaveChanges();
		}

	}
}
