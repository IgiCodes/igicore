using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using CitizenFX.Core;
using IgiCore.Core.Extensions;
using IgiCore.Core.Models.Objects.Vehicles;
using IgiCore.Core.Rpc;
using IgiCore.Server.Rpc;

namespace IgiCore.Server.Controllers
{
    public static class VehicleController
    {
	    public static async void Create<T>([FromSource] Player player, string json) where T : class, IVehicle
	    {
		    var response = RpcResponse<T>.Parse(json);
		    T vehicle = response.Result;

			Server.Db.Set<T>().Add(vehicle);
			await Server.Db.SaveChangesAsync();

			player
				.Event($"igi:{vehicle.VehicleType().Name}:create")
				.Attach(vehicle)
				.Trigger();
		}


		public static async void Save<T>([FromSource] Player player, string json) where T : class, IVehicle // Has no ID
		{
			var response = RpcResponse<T>.Parse(json);

			T vehicle = response.Result;

            if (vehicle.Id == Guid.Empty) vehicle.Id = Server.Db.Set<T>().FirstOrDefault(c => c.Handle == vehicle.Handle)?.Id ?? Guid.Empty;
            if (vehicle.Id == Guid.Empty) return;

            T dbCar = Server.Db.Set<T>().First(c => c.Id == vehicle.Id);
            if (vehicle.TrackingUserId != dbCar.TrackingUserId && dbCar.TrackingUserId != Guid.Empty) return;

			Server.Db.Set<T>().AddOrUpdate(vehicle);

			T dbVehicle = Server.Db.Set<T>().FirstOrDefault(v => v.Id == vehicle.Id) ?? vehicle;

			dbVehicle.Extras = vehicle.Extras;
			dbVehicle.Mods = vehicle.Mods;
			dbVehicle.Doors = vehicle.Doors;
			dbVehicle.Seats = vehicle.Seats;
			dbVehicle.Wheels = vehicle.Wheels;
			dbVehicle.Windows = vehicle.Windows;
			
			await Server.Db.SaveChangesAsync();
        }
    }
}
