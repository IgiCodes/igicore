using System;
using System.Data.Entity.Migrations;
using System.Linq;
using CitizenFX.Core;
using IgiCore.Core.Models.Objects.Vehicles;
using IgiCore.Core.Rpc;

namespace IgiCore.Server.Controllers
{
    public static class VehicleActions
    {
        public static async void Save<T>([FromSource] Player player, string json) where T : class, IVehicle // Has no ID
		{
			var response = RpcResponse<T>.Parse(json);

			T vehicle = response.Result;

            if (vehicle.Id == Guid.Empty) vehicle.Id = Server.Db.Set<T>().FirstOrDefault(c => c.Handle == vehicle.Handle)?.Id ?? Guid.Empty;
            if (vehicle.Id == Guid.Empty) return;

            T dbCar = Server.Db.Set<T>().First(c => c.Id == vehicle.Id);
            if (vehicle.TrackingUserId != dbCar.TrackingUserId && dbCar.TrackingUserId != Guid.Empty) return;

            Server.Db.Set<T>().AddOrUpdate(vehicle);
            await Server.Db.SaveChangesAsync();
        }
    }
}
