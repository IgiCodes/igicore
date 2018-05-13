using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
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

		public List<Tuple<Type, int>> Tracked { get; set; } = new List<Tuple<Type, int>>();

		public override async Task Tick()
		{
			Update();
			Save();

			await BaseScript.Delay(1000);
		}

		private void Update()
		{
			foreach (Tuple<Type, int> trackedVehicle in this.Tracked.ToList())
			{
				int vehicleHandle = API.NetToVeh(trackedVehicle.Item2);
				var citVeh = new CitizenFX.Core.Vehicle(vehicleHandle);
				var closestPlayer = new CitizenFX.Core.Player(API.GetNearestPlayerToEntity(citVeh.Handle));

				if (closestPlayer == Game.Player || !API.NetworkIsPlayerConnected(closestPlayer.Handle))
				{
					if (!(Vector3.Distance(Game.Player.Character.Position, citVeh.Position) > VehicleLoadDistance)) continue;

					citVeh.Delete();
					this.Tracked.Remove(trackedVehicle);
					Server.Event(RpcEvents.CarUnclaim)
						.Attach(trackedVehicle.Item2)
						.Trigger();
				}
				else
				{
					int netId = API.NetworkGetNetworkIdFromEntity(citVeh.Handle);

					Car car = (Car)citVeh;
					car.NetId = netId;

					Client.Log($"Transfering vehicle to player: {closestPlayer.ServerId}  -  {car.Handle}");
					Server.Event(RpcEvents.CarTransfer)
						.Attach(car)
						.Attach(closestPlayer.ServerId)
						.Trigger();
				}
			}
		}

		private void Save()
		{
			foreach (Tuple<Type, int> trackedVehicle in this.Tracked)
			{
				int vehicleHandle = API.NetToVeh(trackedVehicle.Item2);
				var citVeh = new CitizenFX.Core.Vehicle(vehicleHandle);
				int netId = API.NetworkGetNetworkIdFromEntity(citVeh.Handle);

				//Debug.WriteLine($"Saving Vehicle: {trackedVehicle.Item2} - {citVeh.Position}");

				Core.Models.Objects.Vehicles.Vehicle vehicle = (Core.Models.Objects.Vehicles.Vehicle)citVeh;
				vehicle.TrackingUserId = Client.Instance.User.Id;
				vehicle.NetId = netId;
				vehicle.Hash = citVeh.Model.Hash;

				switch (trackedVehicle.Item1.VehicleType().Name)
				{
					case "Car":
						//Car car = (Car)vehicle;
						// Add car specific props...
						//BaseScript.TriggerServerEvent($"igi:{trackedVehicle.Item1.VehicleType().Name}:save", JsonConvert.SerializeObject(car));
						Server.Event($"igi:{trackedVehicle.Item1.VehicleType().Name}:save")
							.Attach(vehicle)
							.Trigger();
						break;

					default:
						//BaseScript.TriggerServerEvent($"igi:{trackedVehicle.Item1.VehicleType().Name}:save", JsonConvert.SerializeObject(vehicle, trackedVehicle.Item1, new JsonSerializerSettings()));
						Server.Event($"igi:{trackedVehicle.Item1.VehicleType().Name}:save")
							.Attach(vehicle)
							.Trigger();
						break;
				}
			}
		}
	}
}
