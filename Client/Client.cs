using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using IgiCore.Client.Extensions;
using IgiCore.Client.Models;
using IgiCore.Core.Models.Objects.Vehicles;
using Newtonsoft.Json;
using IgiCore.Client.Services;
using static CitizenFX.Core.Native.API;
using Vehicle = IgiCore.Core.Models.Objects.Vehicles.Vehicle;

namespace IgiCore.Client
{
    public class Client : BaseScript
    {
        public static User User;

        public new event Func<Task> Tick;

        public new Player LocalPlayer => base.LocalPlayer;

        protected ServiceRegistry Services = new ServiceRegistry()
        {
            new VehicleService()
        };

        public Client()
        {
            // Forward tick event
            base.Tick += () => this.Tick?.Invoke();

            // Notify server that client is loaded
            TriggerServerEvent("igi:client:ready");

            HandleJsonEvent<User>("igi:user:load", UserLoad);

            // Load the user
            TriggerServerEvent("igi:user:load");

            HandleEvent<string>("igi:car:spawn", SpawnVehicle<Car>);
            HandleEvent<string>("igi:car:claim", ClaimVehicle<Car>);
            HandleEvent<string>("igi:car:unclaim", UnclaimVehicle<Car>);

            HandleEvent<string>("igi:bike:spawn", SpawnVehicle<Bike>);
            HandleEvent<string>("igi:bike:claim", ClaimVehicle<Bike>);
            HandleEvent<string>("igi:bike:unclaim", UnclaimVehicle<Bike>);

            // Set pause screen title
            Function.Call(Hash.ADD_TEXT_ENTRY, "FE_THDR_GTAO", "TEST");

            foreach (ClientService service in this.Services)
            {
                this.Tick += async () => await service.Tick(this);
            }
        }

        public void ClaimVehicle<T>(string vehJson) where T : Vehicle
        {
            T vehicle = JsonConvert.DeserializeObject<T>(vehJson);
            Log($"Claiming vehicle with netId: {vehicle.NetId}");
            int vehHandle = NetToVeh(vehicle.NetId ?? 0);
            if (vehHandle == 0) return;
            Log($"Handle found for net id: {vehHandle}");
            CitizenFX.Core.Vehicle citizenVehicle = new CitizenFX.Core.Vehicle(vehHandle);
            VehToNet(citizenVehicle.Handle);
            NetworkRegisterEntityAsNetworked(citizenVehicle.Handle);
            int netId = NetworkGetNetworkIdFromEntity(citizenVehicle.Handle);

            Log($"Sending {vehicle.Id}");

            TriggerServerEvent("igi:car:claim", vehicle.Id.ToString());

            this.Services.First<VehicleService>().Tracked.Add(new Tuple<Type, int>(typeof(T), netId));

            Log($"Tracked vehicle count in claim: {string.Join(", ", this.Services.First<VehicleService>().Tracked)}");
        }

        public void UnclaimVehicle<T>(string vehJson) where T : Vehicle
        {
            T vehicle = JsonConvert.DeserializeObject<T>(vehJson);
            Log($"Unclaiming car: {vehicle.Id} with NetId: {vehicle.NetId}");
            Log($"Currently tracking: {string.Join(", ", this.Services.First<VehicleService>().Tracked)}");
            this.Services.First<VehicleService>().Tracked.Remove(new Tuple<Type, int>(typeof(T), vehicle.NetId ?? 0));
            Log($"Now tracking: {string.Join(", ", this.Services.First<VehicleService>().Tracked)}");
        }

        public async void SpawnVehicle<T>(string vehJson) where T : Vehicle
        {
            T vehToSpawn = JsonConvert.DeserializeObject<T>(vehJson);
            Log($"Spawning {vehToSpawn.Id}");

            var spawnedVehicle = await vehToSpawn.ToCitizenVehicle();
            VehToNet(spawnedVehicle.Handle);
            NetworkRegisterEntityAsNetworked(spawnedVehicle.Handle);
            int netId = NetworkGetNetworkIdFromEntity(spawnedVehicle.Handle);
            //SetNetworkIdExistsOnAllMachines(netId, true);

            Log($"Spawned {spawnedVehicle.Handle} with netId {netId}");

            Vehicle vehicle = spawnedVehicle;
            vehicle.Id = vehToSpawn.Id;
            vehicle.TrackingUserId = User.Id;
            vehicle.Handle = spawnedVehicle.Handle;
            vehicle.NetId = netId;

            string className = typeof(T).BaseType.IsSubclassOf(typeof(Vehicle))
                ? typeof(T).BaseType.Name
                : typeof(T).Name;

            Log($"Sending {vehicle.Id} with event \"igi:{className}:save\"");

            TriggerServerEvent($"igi:{className}:save", JsonConvert.SerializeObject(vehicle, typeof(T), new JsonSerializerSettings()));

            this.Services.First<VehicleService>().Tracked.Add(new Tuple<Type, int>(typeof(T), netId));

            return;
        }

        protected async void UserLoad(User user)
        {
            Assert(user != null, "User param is empty");
            Assert(Client.User == null, "User already loaded");

            // Store the user
            Client.User = user;

            //HandleJsonEvent<Character>("igi:character:new", CharacterLoad); // Does the client care?
            HandleJsonEvent<Character>("igi:character:load", CharacterLoad);

            await SpawnPlayer();
        }

        protected void FreezePlayer(bool freeze)
        {
            Assert(PlayerId() == this.LocalPlayer.Handle, "HANDLE 1");
            Assert(GetPlayerPed(-1) == this.LocalPlayer.Character.Handle, "HANDLE 2");

            Game.Player.CanControlCharacter = !freeze;

            this.LocalPlayer.Character.IsVisible = !freeze;

            if (!this.LocalPlayer.Character.IsInVehicle())
            {
                this.LocalPlayer.Character.IsCollisionEnabled = !freeze;
            }

            this.LocalPlayer.Character.IsPositionFrozen = freeze;

            this.LocalPlayer.Character.IsInvincible = freeze;

            if (!this.LocalPlayer.Character.IsDead && freeze)
            {
                this.LocalPlayer.Character.Task.ClearAllImmediately();
            }
        }

        protected async Task SpawnPlayer()
        {
            Screen.Fading.FadeOut(500);
            while (Screen.Fading.IsFadingOut) await Delay(10);

            FreezePlayer(true);

            // Swap model
            await this.LocalPlayer.ChangeModel(new Model(PedHash.FreemodeMale01));

            // Not naked
            Game.Player.Character.Style.SetDefaultClothes();

            this.LocalPlayer.Character.Position = new Vector3 { X = -1038.121f, Y = -2738.279f, Z = 20.16929f };
            this.LocalPlayer.Character.Resurrect();
            this.LocalPlayer.Character.Task.ClearAllImmediately();
            this.LocalPlayer.Character.Weapons.Drop();
            this.LocalPlayer.WantedLevel = 0;

            this.LocalPlayer.Character.Weapons.Give(WeaponHash.Railgun, 100, true, true);

            ShutdownLoadingScreen();

            Screen.Fading.FadeIn(500);
            while (Screen.Fading.IsFadingIn) await Delay(10);

            FreezePlayer(false);

            // Temporary respawning
            this.Tick += async () =>
            {
                if (!this.LocalPlayer.Character.IsAlive)
                {
                    this.LocalPlayer.Character.Resurrect();
                    this.LocalPlayer.WantedLevel = 0;
                    this.LocalPlayer.Character.IsCollisionEnabled = true;
                }
                await Delay(10);
            };

            Screen.ShowNotification($"{Game.Player.Name} connected at {DateTime.Now:s}");
        }

        protected void CharacterLoad(Character character)
        {
            // Unload old character
            Client.User.Character?.Dispose();

            // Store the character
            Client.User.Character = character ?? throw new ArgumentNullException(nameof(character));
            Client.User.Character.Initialize(this); // Fake ctor

            // Render new character
            Client.User.Character.Render();

            Screen.ShowNotification($"{Client.User.Character.Name} loaded at {DateTime.Now:s}");
        }

        [System.Diagnostics.Conditional("DEBUG")]
        public static void Log(string message)
        {
            Debug.Write($"{DateTime.Now:s} [CLIENT]: {message}");
        }

        [System.Diagnostics.Conditional("DEBUG")]
        public static void Assert(bool condition)
        {
            System.Diagnostics.Debug.Assert(condition);
        }

        [System.Diagnostics.Conditional("DEBUG")]
        public static void Assert(bool condition, string message)
        {
            System.Diagnostics.Debug.Assert(condition, message);
        }

        public void HandleEvent(string name, Action action) => this.EventHandlers[name] += action;
        public void HandleEvent<T1>(string name, Action<T1> action) => this.EventHandlers[name] += action;
        public void HandleEvent<T1, T2>(string name, Action<T1, T2> action) => this.EventHandlers[name] += action;
        public void HandleEvent<T1, T2, T3>(string name, Action<T1, T2, T3> action) => this.EventHandlers[name] += action;
        public void HandleJsonEvent<T>(string eventName, Action<T> action) => this.EventHandlers[eventName] += new Action<string>(json => action(JsonConvert.DeserializeObject<T>(json)));
        public void HandleJsonEvent<T1, T2>(string eventName, Action<T1, T2> action) => this.EventHandlers[eventName] += new Action<string, string>((j1, j2) => action(JsonConvert.DeserializeObject<T1>(j1), JsonConvert.DeserializeObject<T2>(j2)));
        public void HandleJsonEvent<T1, T2, T3>(string eventName, Action<T1, T2, T3> action) => this.EventHandlers[eventName] += new Action<string, string, string>((j1, j2, j3) => action(JsonConvert.DeserializeObject<T1>(j1), JsonConvert.DeserializeObject<T2>(j2), JsonConvert.DeserializeObject<T3>(j3)));
    }
}
