using System;
using System.Data.Entity;
using System.Linq;
using IgiCore.SDK.Core.Diagnostics;
using IgiCore.SDK.Core.Rpc;
using IgiCore.SDK.Server.Controllers;
using IgiCore.SDK.Server.Events;
using IgiCore.SDK.Server.Rpc;
using MySql.Data.MySqlClient.Memcached;
using Roleplay.Vehicles.Core.Extensions;
using Roleplay.Vehicles.Core.Models;
using Roleplay.Vehicles.Server.Storage;

namespace Roleplay.Vehicles.Server.Controllers
{
	public class VehicleController : ConfigurableController<Configuration>
	{
		public VehicleController(ILogger logger, IEventManager events, IRpcHandler rpc, Configuration configuration) : base(logger, events, rpc, configuration)
		{
			this.Logger.Debug(this.Configuration.Test);

			this.Rpc.Event(RpcEvents.CarCreate).On<Car>(Create);
			this.Rpc.Event(RpcEvents.CarSave).On<Car>(Save);

			this.Rpc.Event(RpcEvents.BikeCreate).On<Bike>(Create);
			this.Rpc.Event(RpcEvents.BikeSave).On<Bike>(Save);

		}

		public async void Create<T>(IRpcEvent e, T vehicle) where T : Vehicle
		{
			using (var context = new VehicleContext())
			{
				context.Set<T>().Add(vehicle);
				await context.SaveChangesAsync();
			}
			e.Reply(vehicle);
		}


		public async void Save<T>(IRpcEvent e, T vehicle) where T : Vehicle // Has no ID
		{
			using (var context = new VehicleContext())
			{
				this.Logger.Debug("Vehicle Save 1");
				if (vehicle.Id == Guid.Empty) vehicle.Id = context.Set<T>().FirstOrDefault(c => c.Handle == vehicle.Handle)?.Id ?? Guid.Empty;
				if (vehicle.Id == Guid.Empty) return;
				this.Logger.Debug("Vehicle Save 2");
				this.Logger.Debug($"Looking up vehicle: {vehicle.Id}");
				T dbVeh = context.Set<T>()
					.Include(v => v.Extras)
					.Include(v => v.Wheels)
					.Include(v => v.Doors)
					.Include(v => v.Windows)
					.Include(v => v.Seats)
					.Include(v => v.Mods)
					.FirstOrDefault(c => c.Id == vehicle.Id);


				if (dbVeh == null || vehicle.TrackingUserId != dbVeh.TrackingUserId && dbVeh.TrackingUserId != Guid.Empty) return;

				context.Entry(dbVeh).CurrentValues.SetValues(vehicle);


				// Wheels
				foreach (VehicleWheel dbVehWheel in dbVeh.Wheels.ToList())
					if (vehicle.Wheels.All(m => m.Position != dbVehWheel.Position)) context.VehicleWheels.Remove(dbVehWheel);

				foreach (VehicleWheel vehWheel in vehicle.Wheels)
				{
					var dbVehWheel = dbVeh.Wheels.SingleOrDefault(s => s.Position == vehWheel.Position);
					if (dbVehWheel != null)
					{
						vehWheel.Id = dbVehWheel.Id;
						context.Entry(dbVehWheel).CurrentValues.SetValues(vehWheel);
						// We have to manually set enums for some reason...
						context.Entry(dbVehWheel).Property("Position").CurrentValue = vehWheel.Position;
					}
					else dbVeh.Wheels.Add(vehWheel);
				}

				// Doors
				foreach (VehicleDoor dbVehDoor in dbVeh.Doors.ToList())
					if (vehicle.Doors.All(m => m.Index != dbVehDoor.Index)) context.VehicleDoors.Remove(dbVehDoor);

				foreach (VehicleDoor vehDoor in vehicle.Doors)
				{
					var dbVehDoor = dbVeh.Doors.SingleOrDefault(s => s.Index == vehDoor.Index);
					if (dbVehDoor != null)
					{
						vehDoor.Id = dbVehDoor.Id;
						context.Entry(dbVehDoor).CurrentValues.SetValues(vehDoor);
					}
					else dbVeh.Doors.Add(vehDoor);
				}

				// Extras
				foreach (VehicleExtra dbVehExtra in dbVeh.Extras.ToList())
					if (vehicle.Extras.All(m => m.Index != dbVehExtra.Index)) context.VehicleExtras.Remove(dbVehExtra);

				foreach (VehicleExtra vehExtra in vehicle.Extras)
				{
					var dbVehExtra = dbVeh.Extras.SingleOrDefault(s => s.Index == vehExtra.Index);
					if (dbVehExtra != null)
					{
						vehExtra.Id = dbVehExtra.Id;
						context.Entry(dbVehExtra).CurrentValues.SetValues(vehExtra);
					}
					else dbVeh.Extras.Add(vehExtra);
				}

				// Windows
				foreach (VehicleWindow dbVehWindow in dbVeh.Windows.ToList())
					if (vehicle.Windows.All(m => m.Index != dbVehWindow.Index)) context.VehicleWindows.Remove(dbVehWindow);

				foreach (VehicleWindow vehWindow in vehicle.Windows)
				{
					var dbVehWindow = dbVeh.Windows.SingleOrDefault(s => s.Index == vehWindow.Index);
					if (dbVehWindow != null)
					{
						vehWindow.Id = dbVehWindow.Id;
						context.Entry(dbVehWindow).CurrentValues.SetValues(vehWindow);
					}
					else dbVeh.Windows.Add(vehWindow);
				}

				// Seats
				foreach (VehicleSeat dbVehSeat in dbVeh.Seats.ToList())
					if (vehicle.Seats.All(m => m.Index != dbVehSeat.Index)) context.VehicleSeats.Remove(dbVehSeat);

				foreach (VehicleSeat vehSeat in vehicle.Seats)
				{
					var dbVehSeat = dbVeh.Seats.SingleOrDefault(s => s.Index == vehSeat.Index);
					if (dbVehSeat != null)
					{
						vehSeat.Id = dbVehSeat.Id;
						context.Entry(dbVehSeat).CurrentValues.SetValues(vehSeat);
					}
					else dbVeh.Seats.Add(vehSeat);
				}

				// TODO: Mods

				await context.SaveChangesAsync();

				this.Logger.Debug("Vehicle Save End");
			}
			this.Logger.Debug("Vehicle Save Context Closed");
		}
	}
}
