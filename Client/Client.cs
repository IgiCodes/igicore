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

namespace IgiCore.Client
{
    public class Client : BaseScript
    {
        protected User User;

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

            HandleJsonEvent<Car>("igi:car:spawn", SpawnVehicle);
            HandleJsonEvent<Car>("igi:vehicle:claim", ClaimVehicle);

            // Set pause screen title
            Function.Call(Hash.ADD_TEXT_ENTRY, "FE_THDR_GTAO", "TEST");

            foreach (ClientService service in this.Services)
            {
                this.Tick += async () => await service.Tick(this);
            }
        }

        public void ClaimVehicle(Car car)
        {
            Log($"Claiming vehicle with netId: {car.NetId}");
            int vehHandle = NetToVeh(car.NetId ?? 0);
            Log($"Handle found for net id: {vehHandle}");
            CitizenFX.Core.Vehicle vehicle = new CitizenFX.Core.Vehicle(vehHandle);
            VehToNet(vehicle.Handle);
            NetworkRegisterEntityAsNetworked(vehicle.Handle);
            int netId = NetworkGetNetworkIdFromEntity(vehicle.Handle);

            Car newCar = vehicle;
            newCar.Id = car.Id;
            newCar.NetId = netId;

            Log($"Sending {car.Id}");

            TriggerServerEvent("igi:car:save", JsonConvert.SerializeObject(newCar));

            this.Services.First<VehicleService>().Tracked.Add(vehicle.Handle);
        }

        public async void SpawnVehicle(Car carToSpawn)
        {
            Log($"Spawning {carToSpawn.Id}");

            var veh = await carToSpawn.ToCitizenVehicle();
            VehToNet(veh.Handle);
            NetworkRegisterEntityAsNetworked(veh.Handle);
            int netId = NetworkGetNetworkIdFromEntity(veh.Handle);
            //SetNetworkIdExistsOnAllMachines(netId, true);

            Log($"Spawned {veh.Handle} with netId {netId}");

            Car car = veh;
            car.Id = carToSpawn.Id;
            car.NetId = netId;


            Log($"Sending {car.Id}");

            TriggerServerEvent("igi:car:save", JsonConvert.SerializeObject(car));

            this.Services.First<VehicleService>().Tracked.Add(car.Handle ?? 0);

            return;
        }

        protected async void UserLoad(User user)
        {
            Assert(user != null, "User param is empty");
            Assert(this.User == null, "User already loaded");

            // Store the user
            this.User = user;

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
                }
                await Delay(10);
            };

            Screen.ShowNotification($"{Game.Player.Name} connected at {DateTime.Now:s}");
        }

        protected void CharacterLoad(Character character)
        {
            // Unload old character
            this.User.Character?.Dispose();

            // Store the character
            this.User.Character = character ?? throw new ArgumentNullException(nameof(character));
            this.User.Character.Initialize(this); // Fake ctor

            // Render new character
            this.User.Character.Render();

            Screen.ShowNotification($"{this.User.Character.Name} loaded at {DateTime.Now:s}");
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
