using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using IgiCore.Core.Extensions;
using IgiCore.Core.Models.Objects.Vehicles;
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
					BaseScript.TriggerServerEvent("igi:car:unclaim", trackedVehicle.Item2);
				}
				else
				{
					int netId = API.NetworkGetNetworkIdFromEntity(citVeh.Handle);
					//Debug.WriteLine($"Vehicle: {vehicleHandle} NetId: {netId} - {citVeh.Position}");

					Car car = citVeh;
					car.NetId = netId;

					// Transfer the vehicle to the closest client
					//Client.Log($"Removing Vehicle from tracked: {car.Handle}");
					//this.Tracked.Remove(car.Handle ?? 0);

					Client.Log($"Transfering vehicle to player: {closestPlayer.ServerId}  -  {car.Handle}");
					BaseScript.TriggerServerEvent("igi:car:transfer", JsonConvert.SerializeObject(car), closestPlayer.ServerId);
				}
			}
		}

		private void Save()
		{
			Client.Log("Save called.");
			Client.Log(string.Join(", ", this.Tracked));

			foreach (Tuple<Type, int> trackedVehicle in this.Tracked)
			{
				int vehicleHandle = API.NetToVeh(trackedVehicle.Item2);
				var citVeh = new CitizenFX.Core.Vehicle(vehicleHandle);
				int netId = API.NetworkGetNetworkIdFromEntity(citVeh.Handle);

				Debug.WriteLine($"Saving Vehicle: {trackedVehicle.Item2} - {citVeh.Position}");

				Core.Models.Objects.Vehicles.Vehicle vehicle = citVeh;
				vehicle.TrackingUserId = Client.Instance.User.Id;
				vehicle.NetId = netId;
				vehicle.Hash = citVeh.Model.Hash;

				switch (trackedVehicle.Item1.VehicleType().Name)
				{
					case "Car":
						Car car = (Car)vehicle;
						// Add car specific props...
						BaseScript.TriggerServerEvent($"igi:{trackedVehicle.Item1.VehicleType().Name}:save", JsonConvert.SerializeObject(car));
						break;

					default:
						BaseScript.TriggerServerEvent($"igi:{trackedVehicle.Item1.VehicleType().Name}:save", JsonConvert.SerializeObject(vehicle, trackedVehicle.Item1, new JsonSerializerSettings()));
						break;
				}
			}
		}
	}
}
