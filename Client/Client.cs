using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using IgiCore.Client.Models;
using Newtonsoft.Json;
using static CitizenFX.Core.Native.API;

namespace IgiCore.Client
{
    public class Client : BaseScript
    {
        protected User User;

        public new event Func<Task> Tick;

        public new Player LocalPlayer => base.LocalPlayer;

        public Client()
        {
            // Forward tick event
            base.Tick += () => this.Tick?.Invoke();

            // Notify server that client is loaded
            TriggerServerEvent("igi:client:ready");

            HandleJsonEvent<User>("igi:user:load", UserLoad);

            // Load the user
            TriggerServerEvent("igi:user:load");
        }

        protected async void UserLoad(User user)
        {
            Assert(user != null, "User param is empty");
            Assert(this.User == null, "User already loaded");

            // Store the user
            this.User = user;

            //HandleJsonEvent<Character>("igi:character:new", CharacterLoad); // Does the client care?
            HandleJsonEvent<Character>("igi:character:load", async c => await CharacterLoad(c));

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
            if (!await this.LocalPlayer.ChangeModel(new Model(PedHash.FreemodeMale01))) throw new ExternalException("ChangeModel failed");

            // Not naked
            Game.Player.Character.Style.SetDefaultClothes();

            this.LocalPlayer.Character.Position = new Vector3(-802.311f, 175.056f, 72.8446f);
            this.LocalPlayer.Character.Resurrect();
            this.LocalPlayer.Character.Task.ClearAllImmediately();
            this.LocalPlayer.Character.Weapons.Drop();
            this.LocalPlayer.WantedLevel = 0;

            this.LocalPlayer.Character.Weapons.Give(WeaponHash.AssaultRifle, 100, true, true);

            ShutdownLoadingScreen();

            Screen.Fading.FadeIn(500);
            while (Screen.Fading.IsFadingIn) await Delay(10);

            FreezePlayer(false);

            Screen.ShowNotification($"{Game.Player.Name} connected at {DateTime.Now:s}");
        }

        protected async Task CharacterLoad(Character character)
        {
            Assert(this.User != null, "User is empty");
            Assert(character != null, "Character param is empty");

            if (this.User.Character != null)
            {
                // Unload old character
                this.User.Character.Dispose();
            }

            // Store the character
            this.User.Character = character;
            await this.User.Character.Initialize(this); // Fake ctor

            // Render new character
            this.User.Character.Render();

            Screen.ShowNotification($"{this.User.Character.Name} loaded at {DateTime.Now:s}");
        }

        [System.Diagnostics.Conditional("DEBUG")]
        protected static void Log(string message)
        {
            Debug.WriteLine($"{DateTime.Now:s} [CLIENT]: {message}");
        }

        [System.Diagnostics.Conditional("DEBUG")]
        protected static void Assert(bool condition)
        {
            System.Diagnostics.Debug.Assert(condition);
        }

        [System.Diagnostics.Conditional("DEBUG")]
        protected static void Assert(bool condition, string message)
        {
            System.Diagnostics.Debug.Assert(condition, message);
        }

        public void HandleEvent(string name, Action action) => EventHandlers[name] += action;
        public void HandleEvent<T1>(string name, Action<T1> action) => EventHandlers[name] += action;
        public void HandleEvent<T1, T2>(string name, Action<T1, T2> action) => EventHandlers[name] += action;
        public void HandleEvent<T1, T2, T3>(string name, Action<T1, T2, T3> action) => EventHandlers[name] += action;
        public void HandleJsonEvent<T>(string eventName, Action<T> action) => EventHandlers[eventName] += new Action<string>(json => action(JsonConvert.DeserializeObject<T>(json)));
        public void HandleJsonEvent<T1, T2>(string eventName, Action<T1, T2> action) => EventHandlers[eventName] += new Action<string, string>((j1, j2) => action(JsonConvert.DeserializeObject<T1>(j1), JsonConvert.DeserializeObject<T2>(j2)));
        public void HandleJsonEvent<T1, T2, T3>(string eventName, Action<T1, T2, T3> action) => EventHandlers[eventName] += new Action<string, string, string>((j1, j2, j3) => action(JsonConvert.DeserializeObject<T1>(j1), JsonConvert.DeserializeObject<T2>(j2), JsonConvert.DeserializeObject<T3>(j3)));
    }
}
