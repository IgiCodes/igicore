using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Entity;
using System.Linq;
using CitizenFX.Core;
using IgiCore.Core.Extensions;
using IgiCore.Core.Models.Objects.Vehicles;
using IgiCore.Core.Rpc;
using IgiCore.Server.Rpc;
using Newtonsoft.Json;

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


		public static async void Save<T>([FromSource] Player player, string json) where T : Vehicle // Has no ID
		{
			
			var response = RpcResponse<T>.Parse(json);

			T vehicle = response.Result;

            if (vehicle.Id == Guid.Empty) vehicle.Id = Server.Db.Set<T>().FirstOrDefault(c => c.Handle == vehicle.Handle)?.Id ?? Guid.Empty;
            if (vehicle.Id == Guid.Empty) return;

            T dbVeh = Server.Db.Set<T>()
				.Include(v => v.Extras)
	            .Include(v => v.Wheels)
	            .Include(v => v.Doors)
	            .Include(v => v.Windows)
	            .Include(v => v.Seats)
	            .Include(v => v.Mods)
				.FirstOrDefault(c => c.Id == vehicle.Id);


            if (dbVeh == null || vehicle.TrackingUserId != dbVeh.TrackingUserId && dbVeh.TrackingUserId != Guid.Empty) return;

			Server.Db.Entry(dbVeh).CurrentValues.SetValues(vehicle);


			// Wheels
			foreach (VehicleWheel dbVehWheel in dbVeh.Wheels.ToList())
				if (vehicle.Wheels.All(m => m.Position != dbVehWheel.Position)) Server.Db.VehicleWheels.Remove(dbVehWheel);

			foreach (VehicleWheel vehWheel in vehicle.Wheels)
			{
				var dbVehWheel = dbVeh.Wheels.SingleOrDefault(s => s.Position == vehWheel.Position);
				if (dbVehWheel != null)
				{
					vehWheel.Id = dbVehWheel.Id;
					Server.Db.Entry(dbVehWheel).CurrentValues.SetValues(vehWheel);
					// We have to manually set enums for some reason...
					Server.Db.Entry(dbVehWheel).Property("Position").CurrentValue = vehWheel.Position;
				}
				else dbVeh.Wheels.Add(vehWheel);
			}

			// Doors
			foreach (VehicleDoor dbVehDoor in dbVeh.Doors.ToList())
				if (vehicle.Doors.All(m => m.Index != dbVehDoor.Index)) Server.Db.VehicleDoors.Remove(dbVehDoor);

			foreach (VehicleDoor vehDoor in vehicle.Doors)
			{
				var dbVehDoor = dbVeh.Doors.SingleOrDefault(s => s.Index == vehDoor.Index);
				if (dbVehDoor != null)
				{
					vehDoor.Id = dbVehDoor.Id;
					Server.Db.Entry(dbVehDoor).CurrentValues.SetValues(vehDoor);
				}
				else dbVeh.Doors.Add(vehDoor);
			}

			// Extras
			foreach (VehicleExtra dbVehExtra in dbVeh.Extras.ToList())
				if (vehicle.Extras.All(m => m.Index != dbVehExtra.Index)) Server.Db.VehicleExtras.Remove(dbVehExtra);

			foreach (VehicleExtra vehExtra in vehicle.Extras)
			{
				var dbVehExtra = dbVeh.Extras.SingleOrDefault(s => s.Index == vehExtra.Index);
				if (dbVehExtra != null)
				{
					vehExtra.Id = dbVehExtra.Id;
					Server.Db.Entry(dbVehExtra).CurrentValues.SetValues(vehExtra);
				}
				else dbVeh.Extras.Add(vehExtra);
			}

			// Windows
			foreach (VehicleWindow dbVehWindow in dbVeh.Windows.ToList())
				if (vehicle.Windows.All(m => m.Index != dbVehWindow.Index)) Server.Db.VehicleWindows.Remove(dbVehWindow);

			foreach (VehicleWindow vehWindow in vehicle.Windows)
			{
				var dbVehWindow = dbVeh.Windows.SingleOrDefault(s => s.Index == vehWindow.Index);
				if (dbVehWindow != null)
				{
					vehWindow.Id = dbVehWindow.Id;
					Server.Db.Entry(dbVehWindow).CurrentValues.SetValues(vehWindow);
				}
				else dbVeh.Windows.Add(vehWindow);
			}

			// Seats
			foreach (VehicleSeat dbVehSeat in dbVeh.Seats.ToList())
				if (vehicle.Seats.All(m => m.Index != dbVehSeat.Index)) Server.Db.VehicleSeats.Remove(dbVehSeat);

			foreach (VehicleSeat vehSeat in vehicle.Seats)
			{
				var dbVehSeat = dbVeh.Seats.SingleOrDefault(s => s.Index == vehSeat.Index);
				if (dbVehSeat != null)
				{
					vehSeat.Id = dbVehSeat.Id;
					Server.Db.Entry(dbVehSeat).CurrentValues.SetValues(vehSeat);
				}
				else dbVeh.Seats.Add(vehSeat);
			}

			// TODO: Mods

			await Server.Db.SaveChangesAsync();
        }
    }
}
