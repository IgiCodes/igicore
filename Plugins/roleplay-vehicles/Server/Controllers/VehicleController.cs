using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using IgiCore.SDK.Core.Diagnostics;
using IgiCore.SDK.Core.Models;
using IgiCore.SDK.Core.Rpc;
using IgiCore.SDK.Server.Controllers;
using IgiCore.SDK.Server.Events;
using IgiCore.SDK.Server.Rpc;
using MySql.Data.MySqlClient.Memcached;
using Roleplay.Vehicles.Core.Extensions;
using Roleplay.Vehicles.Core.Models;
using Roleplay.Vehicles.Core.Rpc;
using Roleplay.Vehicles.Server.Storage;

namespace Roleplay.Vehicles.Server.Controllers
{
	public class VehicleController : ConfigurableController<Configuration>
	{
		public VehicleController(ILogger logger, IEventManager events, IRpcHandler rpc, Configuration configuration) : base(logger, events, rpc, configuration)
		{
			this.Logger.Debug(this.Configuration.Test);

			this.Rpc.Event(VehicleRpcEvents.SavePosition).On<Guid, Position>(SavePosition);
			this.Rpc.Event(VehicleRpcEvents.SaveHeading).On<Guid, float>(SaveHeading);
			this.Rpc.Event(VehicleRpcEvents.SaveBodyHealth).On<Guid, float>(SaveBodyHealth);
			this.Rpc.Event(VehicleRpcEvents.SaveBodyHealth).On<Guid, float>(SaveEngineHealth);
			this.Rpc.Event(VehicleRpcEvents.SavePetrolTankHealth).On<Guid, float>(SavePetrolTankHealth);
			this.Rpc.Event(VehicleRpcEvents.SaveDirtLevel).On<Guid, float>(SaveDirtLevel);
			this.Rpc.Event(VehicleRpcEvents.SaveFuelLevel).On<Guid, float>(SaveFuelLevel);
			this.Rpc.Event(VehicleRpcEvents.SaveOilLevel).On<Guid, float>(SaveOilLevel);

			this.Rpc.Event(VehicleRpcEvents.CarCreate).On<Car>(Create);
			this.Rpc.Event(VehicleRpcEvents.CarSave).On<Car>(Save);

			this.Rpc.Event(VehicleRpcEvents.BikeCreate).On<Bike>(Create);
			this.Rpc.Event(VehicleRpcEvents.BikeSave).On<Bike>(Save);

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

		public async void Save<T>(IRpcEvent e, T vehicle) where T : Vehicle
		{
			using (var context = new VehicleContext())
			{
				await Save(context, vehicle);
				await context.SaveChangesAsync();
			}
		}

		public void SavePosition<T>(IRpcEvent e, Guid vehicleId, T value) => SaveProp(vehicleId, "Position", value);
		public void SaveHeading<T>(IRpcEvent e, Guid vehicleId, T value) => SaveProp(vehicleId, "Heading", value);
		public void SaveBodyHealth<T>(IRpcEvent e, Guid vehicleId, T value) => SaveProp(vehicleId, "BodyHealth", value);
		public void SaveEngineHealth<T>(IRpcEvent e, Guid vehicleId, T value) => SaveProp(vehicleId, "EngineHealth", value);
		public void SavePetrolTankHealth<T>(IRpcEvent e, Guid vehicleId, T value) => SaveProp(vehicleId, "PetrolTankHealth", value);
		public void SaveDirtLevel<T>(IRpcEvent e, Guid vehicleId, T value) => SaveProp(vehicleId, "DirtLevel", value);
		public void SaveFuelLevel<T>(IRpcEvent e, Guid vehicleId, T value) => SaveProp(vehicleId, "FuelLevel", value);
		public void SaveOilLevel<T>(IRpcEvent e, Guid vehicleId, T value) => SaveProp(vehicleId, "OilLevel", value);

		public async void SaveProp<T>(Guid vehicleId, string property, T value)
		{
			this.Logger.Debug($"SaveProp called for ID {vehicleId}: Prop - {property} | value - {value}");
			using (var context = new VehicleContext())
			{
				var vehicle = (await context.Vehicles.FirstOrDefaultAsync(v => v.Id == vehicleId));
				if (vehicle == null) return;
				this.Logger.Debug($"Old value: {context.Entry(vehicle).Property(property).CurrentValue}");
				context.Entry(vehicle).Property(property).CurrentValue = value;
				await context.SaveChangesAsync();
			}
		}

		public async Task Save<T>(VehicleContext context, T vehicle) where T : Vehicle
		{
			if (vehicle.Id == Guid.Empty) vehicle.Id = (await context.Set<T>().FirstOrDefaultAsync(c => c.Handle == vehicle.Handle))?.Id ?? Guid.Empty;
			if (vehicle.Id == Guid.Empty) return;

			T dbVeh = await context.Set<T>()
				.Include(v => v.Extras)
				.Include(v => v.Wheels)
				.Include(v => v.Doors)
				.Include(v => v.Windows)
				.Include(v => v.Seats)
				.Include(v => v.Mods)
				.FirstOrDefaultAsync(c => c.Id == vehicle.Id);

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

		}
	}
}
