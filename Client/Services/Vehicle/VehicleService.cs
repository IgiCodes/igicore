using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using IgiCore.Client.Controllers.Player;
using IgiCore.Client.Extensions;
using IgiCore.Client.Rpc;
using IgiCore.Core.Extensions;
using IgiCore.Core.Models.Objects.Vehicles;
using IgiCore.Core.Rpc;
using Newtonsoft.Json;

namespace IgiCore.Client.Services.Vehicle
{
	public class VehicleService : ClientService
	{
		private const int VehicleLoadDistance = 500;

		public List<TrackedVehicle> Tracked { get; set; } = new List<TrackedVehicle>();

		public override async Task Tick()
		{
			await Update();
			await Save();

			await BaseScript.Delay(1000);
		}

		private async Task Update()
		{
			foreach (TrackedVehicle trackedVehicle in this.Tracked.ToList())
			{
				int vehicleHandle = API.NetToVeh(trackedVehicle.NetId);
				var citVeh = new CitizenFX.Core.Vehicle(vehicleHandle);
				var closestPlayer = new CitizenFX.Core.Player(API.GetNearestPlayerToEntity(citVeh.Handle));

				if (closestPlayer == Game.Player || !API.NetworkIsPlayerConnected(closestPlayer.Handle))
				{
					if (!(Vector3.Distance(Game.Player.Character.Position, citVeh.Position) > VehicleLoadDistance)) continue;

					citVeh.Delete();
					this.Tracked.Remove(trackedVehicle);
					Server.Event($"igi:{trackedVehicle.Type.VehicleType().Name}:unclaim")
						.Attach(trackedVehicle.NetId)
						.Trigger();
				}
				else
				{
					int netId = API.NetworkGetNetworkIdFromEntity(citVeh.Handle);

					Car car = (Car)citVeh;
					car.NetId = netId;

					Client.Log($"Transfering vehicle to player: {closestPlayer.ServerId}  -  {car.Handle}");
					Server.Event($"igi:{trackedVehicle.Type.VehicleType().Name}:transfer")
						.Attach(car)
						.Attach(closestPlayer.ServerId)
						.Trigger();
				}
			}
		}

		private async Task Save()
		{
			foreach (TrackedVehicle trackedVehicle in this.Tracked)
			{
				int vehicleHandle = API.NetToVeh(trackedVehicle.NetId);
				var citVeh = new CitizenFX.Core.Vehicle(vehicleHandle);
				int netId = API.NetworkGetNetworkIdFromEntity(citVeh.Handle);

				Core.Models.Objects.Vehicles.Vehicle vehicle = await citVeh.ToVehicle(trackedVehicle.Id);
				vehicle.TrackingUserId = Client.Instance.Controllers.First<UserController>().User.Id;
				vehicle.NetId = netId;
				vehicle.Hash = citVeh.Model.Hash;

				switch (trackedVehicle.Type.VehicleType().Name)
				{
					case "Car":
						//Car car = (Car)vehicle;
						// Add car specific props...
						Server.Event($"igi:{trackedVehicle.Type.VehicleType().Name}:save")
							.Attach(vehicle)
							.Trigger();
						break;

					default:
						Server.Event($"igi:{trackedVehicle.Type.VehicleType().Name}:save")
							.Attach(vehicle)
							.Trigger();
						break;
				}
			}
		}

		public class TrackedVehicle
		{
			public Guid Id { get; set; }
			public int NetId { get; set; }
			public Type Type { get; set; }
		}
	}
}
