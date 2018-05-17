using System;
using System.Threading.Tasks;
using CitizenFX.Core.Native;
using IgiCore.Client.Controllers.Player;
using IgiCore.Client.Extensions;
using IgiCore.Client.Rpc;
using IgiCore.Client.Services.Vehicle;
using IgiCore.Core.Controllers;
using IgiCore.Core.Extensions;
using IgiCore.Core.Models.Objects.Vehicles;
using IgiCore.Core.Rpc;
using Newtonsoft.Json;

namespace IgiCore.Client.Controllers.Objects.Vehicles
{
	public class VehicleController : Controller
	{
		public VehicleController()
		{
			Server.Event(RpcEvents.CarSpawn).On<Car>(async c => await this.Spawn(c));
			Server.Event(RpcEvents.CarClaim).On<Car>(async c => await this.ClaimVehicle(c));
			Server.Event(RpcEvents.CarUnclaim).On<Car>(async c => await this.UnclaimVehicle(c));

			Server.Event(RpcEvents.BikeClaim).On<Bike>(async b => await this.ClaimVehicle(b));
			Server.Event(RpcEvents.BikeUnclaim).On<Bike>(async b => await this.UnclaimVehicle(b));
		}

		public async Task<T> Create<T>(T vehToCreate) where T : Vehicle
		{
			T newVeh = await Server.Event($"igi:{vehToCreate.VehicleType().Name.ToLower()}:create")
				.Attach(vehToCreate)
				.Request<T>();

			return await Spawn(newVeh);
		}

		public async Task<T> Spawn<T>(T vehToSpawn) where T : Vehicle
		{
			Client.Log($"Spawning {vehToSpawn.Id}");

			CitizenFX.Core.Vehicle spawnedVehicle = await vehToSpawn.ToCitizenVehicle();
			API.VehToNet(spawnedVehicle.Handle);
			API.NetworkRegisterEntityAsNetworked(spawnedVehicle.Handle);
			var netId = API.NetworkGetNetworkIdFromEntity(spawnedVehicle.Handle);
			//SetNetworkIdExistsOnAllMachines(netId, true);

			Client.Log($"Spawned {spawnedVehicle.Handle} with netId {netId}");

			Vehicle vehicle = spawnedVehicle;
			vehicle.Id = vehToSpawn.Id;
			vehicle.TrackingUserId = Client.Instance.Controllers.First<UserController>().User.Id;
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

		public async Task ClaimVehicle<T>(T vehicle) where T : Vehicle
		{
			//T vehicle = JsonConvert.DeserializeObject<T>(vehJson);

			Client.Log($"Claiming vehicle with netId: {vehicle.NetId}");

			var vehHandle = API.NetToVeh(vehicle.NetId ?? 0);
			if (vehHandle == 0) return;

			Client.Log($"Handle found for net id: {vehHandle}");

			CitizenFX.Core.Vehicle citizenVehicle = new CitizenFX.Core.Vehicle(vehHandle);
			API.VehToNet(citizenVehicle.Handle);
			API.NetworkRegisterEntityAsNetworked(citizenVehicle.Handle);
			var netId = API.NetworkGetNetworkIdFromEntity(citizenVehicle.Handle);

			Client.Log($"Sending {vehicle.Id}");

			Server.Event(RpcEvents.CarClaim)
				.Attach(vehicle.Id)
				.Trigger();

			Client.Instance.Services.First<VehicleService>().Tracked.Add(new Tuple<Type, int>(typeof(T), netId));

			Client.Log($"Tracked vehicle count in claim: {string.Join(", ", Client.Instance.Services.First<VehicleService>().Tracked)}");
		}

		public async Task UnclaimVehicle<T>(T vehicle) where T : Vehicle
		{
			//T vehicle = JsonConvert.DeserializeObject<T>(vehJson);

			Client.Log($"Unclaiming car: {vehicle.Id} with NetId: {vehicle.NetId}");
			Client.Log($"Currently tracking: {string.Join(", ", Client.Instance.Services.First<VehicleService>().Tracked)}");

			Client.Instance.Services.First<VehicleService>().Tracked.Remove(new Tuple<Type, int>(typeof(T), vehicle.NetId ?? 0));

			Client.Log($"Now tracking: {string.Join(", ", Client.Instance.Services.First<VehicleService>().Tracked)}");
		}
	}
}
