using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using IgiCore.SDK.Client.Events;
using IgiCore.SDK.Client.Extensions;
using IgiCore.SDK.Client.Input;
using IgiCore.SDK.Client.Interface;
using IgiCore.SDK.Client.Rpc;
using IgiCore.SDK.Client.Services;
using IgiCore.SDK.Core.Diagnostics;
using IgiCore.SDK.Core.Helpers;
using IgiCore.SDK.Core.Models;
using IgiCore.SDK.Core.Models.Player;
using IgiCore.SDK.Core.Rpc;
using JetBrains.Annotations;
using Roleplay.Vehicles.Client.Extensions;
using Roleplay.Vehicles.Core.Extensions;
using Roleplay.Vehicles.Core.Models;
using Roleplay.Vehicles.Core.Rpc;
using Vehicle = Roleplay.Vehicles.Core.Models.Vehicle;

namespace Roleplay.Vehicles.Client
{
	[PublicAPI]
	public class VehicleService : Service
	{
		private const int VehicleLoadDistance = 500;
		public VehicleListCollection Tracked { get; set; } = new VehicleListCollection();
		public Dictionary<int, CitizenFX.Core.Vehicle> CitTracked { get; set; } = new Dictionary<int, CitizenFX.Core.Vehicle>();

		public VehicleService(ILogger logger, ITickManager ticks, IEventManager events, IRpcHandler rpc, INuiManager nui, User user) : base(logger, ticks, events, rpc, nui, user) { }

		public override async Task Loaded()
		{
			// Runs when the service is initialised.
			this.Logger.Debug("Loaded vehicle plugin!");
			await Task.FromResult(0);
		}

		public override async Task Started()
		{
			this.Logger.Debug("Attaching ticks!");
			this.Ticks.Attach(Tick);
			this.Ticks.Attach(Commands);

			await Task.FromResult(0);
		}

		public async Task Commands()
		{
			if (Game.IsControlJustPressed(0, Control.InteractionMenu))
			{
				this.Logger.Debug("Key Pressed!");
				Car car = new Car
				{
					Id = GuidGenerator.GenerateTimeBasedGuid(),
					Hash = (uint)Core.Models.VehicleHash.Elegy,
					Position = Game.PlayerPed.Position.ToPosition(),
					PrimaryColor = new Core.Models.VehicleColor
					{
						StockColor = VehicleStockColor.HotPink,
						CustomColor = new Color(),
						IsCustom = false
					},
					SecondaryColor = new Core.Models.VehicleColor
					{
						StockColor = VehicleStockColor.MattePurple,
						CustomColor = new Color(),
						IsCustom = false
					},
					PearescentColor = VehicleStockColor.HotPink,
					Seats = new List<Core.Models.VehicleSeat>(),
					Wheels = new List<Core.Models.VehicleWheel>(),
					Windows = new List<Core.Models.VehicleWindow>(),
					Doors = new List<Core.Models.VehicleDoor>()
				};

				await Create(car);
			}
		}

		public async Task Tick()
		{
			await Update();
			await Save();

			await BaseScript.Delay(1000);
		}

		private async Task Update()
		{
			await Task.FromResult(0);
			//foreach (TrackedVehicle trackedVehicle in this.Tracked.ToList())
			//{
			//	int vehicleHandle = API.NetToVeh(trackedVehicle.NetId);
			//	var citVeh = new CitizenFX.Core.Vehicle(vehicleHandle);
			//	var closestPlayer = new Player(API.GetNearestPlayerToEntity(citVeh.Handle));

			//	if (closestPlayer == Game.Player || !API.NetworkIsPlayerConnected(closestPlayer.Handle))
			//	{
			//		if (!(Vector3.Distance(Game.Player.Character.Position, citVeh.Position) > VehicleLoadDistance)) continue;

			//		citVeh.Delete();
			//		this.Tracked.Remove(trackedVehicle);
			//		this.Rpc.Event($"igi:{trackedVehicle.Type.VehicleType().Name}:unclaim")
			//			.Trigger(trackedVehicle.NetId);
			//	}
			//	else
			//	{
			//		int netId = API.NetworkGetNetworkIdFromEntity(citVeh.Handle);

			//		Car car = citVeh.ToCar();
			//		car.NetId = netId;

			//		this.Logger.Debug($"Transfering vehicle to player: {closestPlayer.ServerId}  -  {car.Handle}");
			//		this.Rpc.Event($"igi:{trackedVehicle.Type.VehicleType().Name}:transfer")
			//			.Trigger(car, closestPlayer.ServerId);
			//	}
			//}
		}

		private async Task Save()
		{
			this.Logger.Debug("Save() Called");

			// TODO: Batching

			this.Tracked.Cars.ForEach(c =>
			{
				SaveVehicle(c);
				//  Extra car specific checks
			});
			this.Tracked.Bikes.ForEach(c =>
			{
				SaveVehicle(c);
				//  Extra bike specific checks
			});

			await Task.FromResult(0);
		}

		private async void SaveVehicle<T>(T vehicle) where T : Vehicle
		{
			if (!CitTracked.TryGetValue(vehicle.Handle ?? 0, out var citVeh))
			{
				citVeh = new CitizenFX.Core.Vehicle(vehicle.Handle ?? 0);
				CitTracked.Add(citVeh.Handle, citVeh);
			}

			if (vehicle.Position != citVeh.Position.ToPosition()) vehicle.Position = await SaveProp(vehicle.Id, citVeh.Position.ToPosition(), VehicleRpcEvents.SavePosition);
			if (Math.Abs(vehicle.Heading - citVeh.Heading) > 0.01) vehicle.Heading = await SaveProp(vehicle.Id, citVeh.Heading, VehicleRpcEvents.SaveHeading);
			if (Math.Abs(vehicle.BodyHealth - citVeh.BodyHealth) > 0.01) vehicle.BodyHealth = await SaveProp(vehicle.Id, citVeh.BodyHealth, VehicleRpcEvents.SaveBodyHealth);
			if (Math.Abs(vehicle.EngineHealth - citVeh.EngineHealth) > 0.01) vehicle.EngineHealth = await SaveProp(vehicle.Id, citVeh.EngineHealth, VehicleRpcEvents.SaveEngineHealth);
			if (Math.Abs(vehicle.PetrolTankHealth - citVeh.PetrolTankHealth) > 0.01) vehicle.PetrolTankHealth = await SaveProp(vehicle.Id, citVeh.PetrolTankHealth, VehicleRpcEvents.SavePetrolTankHealth);
			if (Math.Abs(vehicle.DirtLevel - citVeh.DirtLevel) > 0.01) vehicle.DirtLevel = await SaveProp(vehicle.Id, citVeh.DirtLevel, VehicleRpcEvents.SaveDirtLevel);
			if (Math.Abs(vehicle.FuelLevel - citVeh.FuelLevel) > 0.01) vehicle.FuelLevel = await SaveProp(vehicle.Id, citVeh.FuelLevel, VehicleRpcEvents.SaveFuelLevel);
			if (Math.Abs(vehicle.OilLevel - citVeh.OilLevel) > 0.01) vehicle.OilLevel = await SaveProp(vehicle.Id, citVeh.OilLevel, VehicleRpcEvents.SaveOilLevel);
		}

		private async Task<T> SaveProp<T>(Guid id, T newVal, string rpcEvent)
		{
			this.Rpc.Event(rpcEvent)
				.Trigger(id, newVal);
			return await Task.FromResult(newVal);
		}

		public async Task Create<T>(T vehToCreate) where T : Vehicle, new()
		{
			T newVeh = await this.Rpc.Event($"igi:{vehToCreate.VehicleType().Name.ToLower()}:create")
				.Request<T>(vehToCreate);

			await Spawn(newVeh);
		}

		public async Task Spawn<T>(T vehToSpawn) where T : Vehicle, new()
		{
			this.Logger.Debug($"Spawning {vehToSpawn.Id}");

			CitizenFX.Core.Vehicle spawnedVehicle = await vehToSpawn.ToCitizenVehicle();

			API.VehToNet(spawnedVehicle.Handle);
			API.NetworkRegisterEntityAsNetworked(spawnedVehicle.Handle);
			var netId = API.NetworkGetNetworkIdFromEntity(spawnedVehicle.Handle);
			//SetNetworkIdExistsOnAllMachines(netId, true);

			this.Logger.Debug($"Spawned {spawnedVehicle.Handle} with netId {netId}");

			T vehicle = await spawnedVehicle.ToVehicle<T>(vehToSpawn.Id);
			vehicle.TrackingUserId = this.User.Id;
			vehicle.Handle = spawnedVehicle.Handle;
			vehicle.NetId = netId;

			this.Logger.Debug($"Sending {vehicle.Id} with event \"igi:{typeof(T).VehicleType().Name}:save\"");

			this.Rpc.Event($"igi:{typeof(T).VehicleType().Name}:save")
				.Trigger(vehicle);

			this.Tracked.Set<T>().Add(vehicle);
			this.CitTracked.Add(spawnedVehicle.Handle, spawnedVehicle);
			this.Logger.Debug($"Added {vehicle.Id} to {typeof(T).VehicleType().Name} list");
			this.Logger.Debug("Tracked list is now:");
			this.Logger.Debug(new Serializer().Serialize(this.Tracked.Set<T>()));
		}

		public class TrackedVehicle
		{
			public Guid Id { get; set; }
			public int NetId { get; set; }
			public Type Type { get; set; }
		}

	}
}
