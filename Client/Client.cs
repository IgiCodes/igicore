using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using IgiCore.Client.Models;
using Newtonsoft.Json;

namespace IgiCore.Client
{
    public class Client : BaseScript
    {
        public new event Func<Task> Tick;

        public User User;

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

        private void UserLoad(User user)
        {
            Log("UserLoad");

            Assert(user != null, "User param is empty");
            Assert(this.User == null, "User already loaded");

            // Store the user
            this.User = user;

            //HandleJsonEvent<Character>("igi:character:new", CharacterLoad); // Does the client care?
            HandleJsonEvent<Character>("igi:character:load", async c => await CharacterLoad(c));
        }

        private async Task CharacterLoad(Character character)
        {
            Log("CharacterLoad");

            Assert(this.User != null, "User is empty");
            Assert(character != null, "Character param is empty");

            if (this.User.Character != null)
            {
                Log("Unloading existing Character");

                // Unload old character
                this.User.Character.Dispose();
            }
            else
            {
                Log("Loading Character for first time");
            }

            // Store the character
            this.User.Character = character;
            await this.User.Character.Initialize(this); // Fake ctor

            // Render new character
            this.User.Character.Render();
        }

        [System.Diagnostics.Conditional("DEBUG")]
        protected static void Log(string message)
        {
            Debug.WriteLine($"{DateTime.UtcNow:s} [CLIENT]: {message}");
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
