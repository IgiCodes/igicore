using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using IgiCore.Client.Extensions;
using IgiCore.Client.Rpc;
using IgiCore.Client.Services.Vehicle;
using IgiCore.Core.Extensions;
using IgiCore.Core.Models.Objects.Vehicles;
using Newtonsoft.Json;

namespace IgiCore.Client.Controllers
{
	public static class VehicleController
	{
		public static async Task<T> Create<T>(T vehToCreate) where T : Core.Models.Objects.Vehicles.Vehicle
		{
			T newVeh = await Server.Event($"igi:{vehToCreate.VehicleType().Name.ToLower()}:create")
				.Attach(vehToCreate)
				.Request<T>();

			return await Spawn(newVeh);
		}

		public static async Task<T> Spawn<T>(T vehToSpawn) where T : Core.Models.Objects.Vehicles.Vehicle
		{
			Client.Log($"Spawning {vehToSpawn.Id}");

			CitizenFX.Core.Vehicle spawnedVehicle = await vehToSpawn.ToCitizenVehicle();
			API.VehToNet(spawnedVehicle.Handle);
			API.NetworkRegisterEntityAsNetworked(spawnedVehicle.Handle);
			var netId = API.NetworkGetNetworkIdFromEntity(spawnedVehicle.Handle);
			//SetNetworkIdExistsOnAllMachines(netId, true);

			Client.Log($"Spawned {spawnedVehicle.Handle} with netId {netId}");

			Core.Models.Objects.Vehicles.Vehicle vehicle = spawnedVehicle;
			vehicle.Id = vehToSpawn.Id;
			vehicle.TrackingUserId = Client.Instance.User.Id;
			vehicle.Handle = spawnedVehicle.Handle;
			vehicle.NetId = netId;

			Client.Log($"Sending {vehicle.Id} with event \"igi:{typeof(T).VehicleType().Name}:save\"");

			Server.Event($"igi:{typeof(T).VehicleType().Name}:save")
				.Attach(vehicle)
				.Trigger();
			//TriggerServerEvent($"igi:{typeof(T).VehicleType().Name}:save", JsonConvert.SerializeObject(vehicle, typeof(T), new JsonSerializerSettings()));

			Client.Instance.Services.First<VehicleService>().Tracked.Add(new Tuple<Type, int>(typeof(T), netId));

			return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(vehicle));
		}
	}
}
