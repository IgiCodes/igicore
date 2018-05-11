using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using CitizenFX.Core;
using IgiCore.Client.Events;
using IgiCore.Client.Interface;
using IgiCore.Client.Interface.Hud;
using IgiCore.Client.Interface.Menu;
using IgiCore.Client.Managers;
using IgiCore.Client.Models;
using IgiCore.Client.Rpc;
using IgiCore.Client.Services;
using IgiCore.Client.Services.AI;
using IgiCore.Client.Services.Economy;
using IgiCore.Client.Services.Player;
using IgiCore.Client.Services.Vehicle;
using IgiCore.Client.Services.World;
using IgiCore.Core;
using IgiCore.Core.Models.Connection;
using IgiCore.Core.Rpc;
using JetBrains.Annotations;
using Debug = CitizenFX.Core.Debug;
using Screen = CitizenFX.Core.UI.Screen;

namespace IgiCore.Client
{
	[PublicAPI]
	public class Client : BaseScript
	{
		/// <summary>
		/// Gets or sets the global singleton instance reference.
		/// </summary>
		/// <value>
		/// The singleton <see cref="Client"/> instance.
		/// </value>
		public static Client Instance { get; protected set; }

		public event EventHandler<ServerInformationEventArgs> OnClientReady;
		public event EventHandler<UserEventArgs> OnUserLoaded;
		public event EventHandler<CharactersEventArgs> OnCharactersList;
		public event EventHandler<CharacterEventArgs> OnCharacterLoaded;

		public ManagerRegistry Managers { get; protected set; }

		public ServiceRegistry Services { get; protected set; }

		public EventHandlerDictionary Handlers => this.EventHandlers;

		/// <summary>
		/// Gets or sets the currently loaded user.
		/// </summary>
		/// <value>
		/// The loaded user.
		/// </value>
		public User User { get; protected set; }

		/// <summary>
		/// Primary client entrypoint.
		/// Initializes a new instance of the <see cref="Client"/> class.
		/// </summary>
		public Client()
		{
			// -- INIT
			Log("Init");

			// Singleton
			Instance = this;

			this.Managers = new ManagerRegistry
			{
				new HudManager(), // Resets and hides all HUD elements
				new MapManager(), // Loads IPLs and blips
				new MenuManager() // Set initial menu options
			};

			this.Services = new ServiceRegistry
			{
				new VehicleRollService(), // Disable rolling cars back over
				new PlayerDeathService(), // Knock down players rather than death
				new PlayerIdleService(), // Kick idle players
				new PedFilterService(), // Block blacklisted peds
				new AiPoliceService(), // Disable AI police
				new PlayerIndicatorService(), // Show nearby players
				new DateTimeService(), // Set the date and time
				new BlackoutService(), // Allow city blackouts
				new AtmService(), // Add ATMs
                new BranchService()
			};

			this.Services.Initialize(); // Attach handlers

			// -- SERVICE EVENTS

			// Player Death Service
			this.Services.First<PlayerDeathService>().OnDowned += (s, e) =>
			{
				UI.ShowNotification("Downed");

				if (this.LocalPlayer.Character.Weapons.Current.Group != WeaponGroup.Unarmed) this.LocalPlayer.Character.Weapons.Remove(this.LocalPlayer.Character.Weapons.Current);
			};

			this.Services.First<PlayerDeathService>().OnRevived += (s, e) =>
			{
				Screen.ShowNotification("Revived");
			};

			//HandleEvent("igi:character:revive", this.Services.First<PlayerDeathService>().Revive);

			//HandleEvent<string>("igi:car:spawn", SpawnVehicle<Car>);
			//HandleEvent<string>("igi:car:claim", ClaimVehicle<Car>);
			//HandleEvent<string>("igi:car:unclaim", UnclaimVehicle<Car>);
			//HandleEvent<string>("igi:bike:spawn", SpawnVehicle<Bike>);
			//HandleEvent<string>("igi:bike:claim", ClaimVehicle<Bike>);
			//HandleEvent<string>("igi:bike:unclaim", UnclaimVehicle<Bike>);

			Startup();
		}

		/// <summary>
		/// Loads initial data from the server, raises events and attaches handlers.
		/// </summary>
		public async Task Startup()
		{
			Log("Startup");

			// Load server details
			this.OnClientReady?.Invoke(this, new ServerInformationEventArgs(await Server
				.Event(RpcEvents.GetServerInformation)
				.Request<ServerInformation>()
			));

			// Load user
			this.User = await Server
				.Event(RpcEvents.GetUser)
				.Request<User>();

			this.OnUserLoaded?.Invoke(this, new UserEventArgs(this.User));

			// Load user's characters
			this.OnCharactersList?.Invoke(this, new CharactersEventArgs(await Server
				.Event(RpcEvents.GetCharacters)
				.Request<List<Character>>()
			));

			Server
				.Event(RpcEvents.GetCharacters)
				.On<List<Character>>(list =>
				{
					Log($"GOT {list.Count} CHARS");

					this.OnCharactersList?.Invoke(this, new CharactersEventArgs(list));
				});

			Server
				.Event(RpcEvents.CharacterLoad)
				.On<Character>(CharacterLoad);

			Log("Waiting for character selection...");
		}

		/// <summary>
		/// Event: igi:character:load
		/// Raises <see cref="OnCharacterLoaded"/> with the loaded and initialized character.
		/// </summary>
		/// <param name="character">The loaded character.</param>
		protected async void CharacterLoad(Character character)
		{
			Log("igi:character:load");

			// Unload old character
			this.User.Character?.Dispose();

			// Store the character
			this.User.Character = character ?? throw new ArgumentNullException(nameof(character));

			// Setup character
			this.User.Character.Initialize();

			// Render new character
			this.User.Character.Render();

			this.OnCharacterLoaded?.Invoke(this, new CharacterEventArgs(this.User.Character));



			//// Spawn character
			//await Game.Player.Spawn(this.User.Character.Position);

			//DisableAutomaticRespawn(true);
			//Game.Player.Character.DropsWeaponsOnDeath = false;
			//Game.Player.Character.Health = Game.Player.Character.MaxHealth;

			//// Enable PvP
			//NetworkSetFriendlyFireOption(true);
			//SetCanAttackFriendly(Game.Player.Character.Handle, true, false);

			//this.Managers.First<HudManager>().Visible = true;






			//var bone = Game.Player.Character.Bones[Bone.PH_R_Hand];

			//var board = await World.CreateProp(new Model(GetHashKey("prop_police_id_board")), Game.Player.Character.Position, Game.Player.Character.Rotation, false, false);
			//board.AttachTo(bone);

			//var text = await World.CreateProp(new Model(GetHashKey("prop_police_id_board")), board.Position, board.Rotation, false, false);
			//text.AttachTo(board);

			//Game.Player.Character.Task.PlayAnimation("mp_character_creation@lineup@male_a", "loop_raised", 1, -1, AnimationFlags.Loop);

			//var scaleformHandle = new Scaleform("mugshot_board_01");
			//scaleformHandle.CallFunction("SET_BOARD", "Los Santos Police Department", "002134234", this.User.Character.FullName, DateTime.Now.ToString("d"), 0);


			//var renderTargetName = "ID_Text";
			//int renderTargetHash = -955488312;
			////RegisterNamedRendertarget(renderTargetName, true);
			////LinkNamedRendertarget((uint) new Model(GetHashKey("prop_police_id_board")).Hash);
			////var renderTargetID = GetNamedRendertargetRenderId(renderTargetName);
			////var renderTargetID = CreateNamedRenderTargetForModel(renderTargetName, text);


			//if (!IsNamedRendertargetRegistered(renderTargetName)) RegisterNamedRendertarget(renderTargetName, true);
			//if (!Function.Call<bool>((Hash)0x113750538FA31298, renderTargetHash)) Function.Call((Hash)0xF6C09E276AEB3F2D, renderTargetHash);// IsNamedRendertargetLinked LinkNamedRendertarget(-955488312);

			//var renderTargetID = IsNamedRendertargetRegistered(renderTargetName) ? GetNamedRendertargetRenderId(renderTargetName) : 0;



			//AttachTickHandler(async () =>
			//{
			//	SetTextRenderId(renderTargetID);

			//	scaleformHandle.CallFunction("SET_BOARD", "Los Santos Police Department", "543-01-1349", DateTime.Now.ToString("d"), this.User.Character.FullName, 0);

			//	//DrawScaleformMovie(scale.Handle, 0.405f, 0.37f, 0.81f, 0.74f, 255, 255, 255, 255, 1);
			//	//scale.Render3D(text.Position - new Vector3(0.022f, 0, 0), text.Rotation - new Vector3(0.022f, 0, 0), new Vector3(0.35f, 0.15f, 0));

			//	SetTextRenderId(GetDefaultScriptRendertargetRenderId());
			//});

			////Screen.ShowNotification($"{this.User.Character.FullName} loaded at {DateTime.Now:h:mm:ss tt}");
		}

		[Conditional("DEBUG")]
		public static void Log(string message) => Debug.Write($"{DateTime.Now:s} [CLIENT]: {message}");

		public void AttachTickHandler(Func<Task> task) => this.Tick += task;

		public void DettachTickHandler(Func<Task> task) => this.Tick -= task;

		//public void ClaimVehicle<T>(string vehJson) where T : Vehicle
		//{
		//	T vehicle = JsonConvert.DeserializeObject<T>(vehJson);

		//	Log($"Claiming vehicle with netId: {vehicle.NetId}");

		//	var vehHandle = NetToVeh(vehicle.NetId ?? 0);
		//	if (vehHandle == 0) return;

		//	Log($"Handle found for net id: {vehHandle}");

		//	CitizenFX.Core.Vehicle citizenVehicle = new CitizenFX.Core.Vehicle(vehHandle);
		//	VehToNet(citizenVehicle.Handle);
		//	NetworkRegisterEntityAsNetworked(citizenVehicle.Handle);
		//	var netId = NetworkGetNetworkIdFromEntity(citizenVehicle.Handle);

		//	Log($"Sending {vehicle.Id}");

		//	TriggerServerEvent("igi:car:claim", vehicle.Id.ToString());

		//	this.Services.First<VehicleService>().Tracked.Add(new Tuple<Type, int>(typeof(T), netId));

		//	Log($"Tracked vehicle count in claim: {string.Join(", ", this.Services.First<VehicleService>().Tracked)}");
		//}

		//public void UnclaimVehicle<T>(string vehJson) where T : Vehicle
		//{
		//	T vehicle = JsonConvert.DeserializeObject<T>(vehJson);

		//	Log($"Unclaiming car: {vehicle.Id} with NetId: {vehicle.NetId}");
		//	Log($"Currently tracking: {string.Join(", ", this.Services.First<VehicleService>().Tracked)}");

		//	this.Services.First<VehicleService>().Tracked.Remove(new Tuple<Type, int>(typeof(T), vehicle.NetId ?? 0));

		//	Log($"Now tracking: {string.Join(", ", this.Services.First<VehicleService>().Tracked)}");
		//}

		//public async void SpawnVehicle<T>(string vehJson) where T : Vehicle
		//{
		//	T vehToSpawn = JsonConvert.DeserializeObject<T>(vehJson);
		//	Log($"Spawning {vehToSpawn.Id}");

		//	CitizenFX.Core.Vehicle spawnedVehicle = await vehToSpawn.ToCitizenVehicle();
		//	VehToNet(spawnedVehicle.Handle);
		//	NetworkRegisterEntityAsNetworked(spawnedVehicle.Handle);
		//	var netId = NetworkGetNetworkIdFromEntity(spawnedVehicle.Handle);
		//	//SetNetworkIdExistsOnAllMachines(netId, true);

		//	Log($"Spawned {spawnedVehicle.Handle} with netId {netId}");

		//	Vehicle vehicle = spawnedVehicle;
		//	vehicle.Id = vehToSpawn.Id;
		//	vehicle.TrackingUserId = this.User.Id;
		//	vehicle.Handle = spawnedVehicle.Handle;
		//	vehicle.NetId = netId;

		//	Log($"Sending {vehicle.Id} with event \"igi:{typeof(T).VehicleType().Name}:save\"");

		//	TriggerServerEvent($"igi:{typeof(T).VehicleType().Name}:save", JsonConvert.SerializeObject(vehicle, typeof(T), new JsonSerializerSettings()));

		//	this.Services.First<VehicleService>().Tracked.Add(new Tuple<Type, int>(typeof(T), netId));
		//}
	}
}
