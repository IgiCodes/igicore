using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using IgiCore.SDK.Client.Events;
using IgiCore.SDK.Client.Extensions;
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
using Vehicle = Roleplay.Vehicles.Core.Models.Vehicle;

namespace Roleplay.Vehicles.Client
{
	[PublicAPI]
	public class VehicleService : Service
	{
		private const int VehicleLoadDistance = 500;
		public List<TrackedVehicle> Tracked { get; set; } = new List<TrackedVehicle>();


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
			foreach (TrackedVehicle trackedVehicle in this.Tracked.ToList())
			{
				int vehicleHandle = API.NetToVeh(trackedVehicle.NetId);
				var citVeh = new CitizenFX.Core.Vehicle(vehicleHandle);
				var closestPlayer = new Player(API.GetNearestPlayerToEntity(citVeh.Handle));

				if (closestPlayer == Game.Player || !API.NetworkIsPlayerConnected(closestPlayer.Handle))
				{
					if (!(Vector3.Distance(Game.Player.Character.Position, citVeh.Position) > VehicleLoadDistance)) continue;

					citVeh.Delete();
					this.Tracked.Remove(trackedVehicle);
					this.Rpc.Event($"igi:{trackedVehicle.Type.VehicleType().Name}:unclaim")
						.Trigger(trackedVehicle.NetId);
				}
				else
				{
					int netId = API.NetworkGetNetworkIdFromEntity(citVeh.Handle);

					Car car = citVeh.ToCar();
					car.NetId = netId;

					this.Logger.Debug($"Transfering vehicle to player: {closestPlayer.ServerId}  -  {car.Handle}");
					this.Rpc.Event($"igi:{trackedVehicle.Type.VehicleType().Name}:transfer")
						.Trigger(car, closestPlayer.ServerId);
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

				Vehicle vehicle = await citVeh.ToVehicle(trackedVehicle.Id);

				vehicle.TrackingUserId = this.User.Id;
				vehicle.NetId = netId;
				vehicle.Hash = citVeh.Model.Hash;

				switch (trackedVehicle.Type.VehicleType().Name)
				{
					case "Car":
						//Car car = new Car(vehicle);
						// Add car specific props...
						this.Rpc.Event($"igi:{trackedVehicle.Type.VehicleType().Name}:save")
							.Trigger(vehicle);
						break;

					default:
						this.Rpc.Event($"igi:{trackedVehicle.Type.VehicleType().Name}:save")
							.Trigger(vehicle);
						break;
				}
			}
		}

		public async Task Create<T>(T vehToCreate) where T : Vehicle
		{
			T newVeh = await this.Rpc.Event($"igi:{vehToCreate.VehicleType().Name.ToLower()}:create")
				.Request<T>(vehToCreate);

			await Spawn(newVeh);
		}

		public async Task Spawn<T>(T vehToSpawn) where T : Vehicle
		{
			this.Logger.Debug($"Spawning {vehToSpawn.Id}");

			CitizenFX.Core.Vehicle spawnedVehicle = await vehToSpawn.ToCitizenVehicle();

			API.VehToNet(spawnedVehicle.Handle);
			API.NetworkRegisterEntityAsNetworked(spawnedVehicle.Handle);
			var netId = API.NetworkGetNetworkIdFromEntity(spawnedVehicle.Handle);
			//SetNetworkIdExistsOnAllMachines(netId, true);

			this.Logger.Debug($"Spawned {spawnedVehicle.Handle} with netId {netId}");

			Vehicle vehicle = await spawnedVehicle.ToVehicle(vehToSpawn.Id);
			vehicle.TrackingUserId = this.User.Id;
			vehicle.Handle = spawnedVehicle.Handle;
			vehicle.NetId = netId;

			this.Logger.Debug($"Sending {vehicle.Id} with event \"igi:{typeof(T).VehicleType().Name}:save\"");

			this.Rpc.Event($"igi:{typeof(T).VehicleType().Name}:save")
				.Trigger(vehicle);
			//TriggerServerEvent($"igi:{typeof(T).VehicleType().Name}:save", JsonConvert.SerializeObject(vehicle, typeof(T), new JsonSerializerSettings()));

			this.Tracked.Add(new TrackedVehicle
			{
				Id = vehicle.Id,
				Type = typeof(T),
				NetId = vehicle.NetId ?? 0
			});
		}

		public class TrackedVehicle
		{
			public Guid Id { get; set; }
			public int NetId { get; set; }
			public Type Type { get; set; }
		}

	}
}
