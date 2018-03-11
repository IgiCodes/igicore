using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using IgiCore.Client.Models;
using Newtonsoft.Json;
using Citizen = CitizenFX.Core.Player;

namespace IgiCore.Client
{
    public class Client : BaseScript
    {
        public User User;
        public Citizen Citizen => LocalPlayer;

        public event Func<Task> CharTick;

        public Client()
        {
            RegisterEvents();

            Tick += () => CharTick?.Invoke();

            TriggerServerEvent("igi:user:load");
        }

        protected void HandleJsonEvent<T>(string eventName, Action<T> action)
        {
            EventHandlers[eventName] += new Action<string>(json =>
            {
                Debug.Write(json);
                action(JsonConvert.DeserializeObject<T>(json));
            });
        }
        protected void HandleJsonEvent<T1, T2>(string eventName, Action<T1, T2> action)
        {
            EventHandlers[eventName] += new Action<string, string>((j1, j2) =>
            {
                action(JsonConvert.DeserializeObject<T1>(j1), JsonConvert.DeserializeObject<T2>(j2));
            });
        }
        protected void HandleJsonEvent<T1, T2, T3>(string eventName, Action<T1, T2, T3> action)
        {
            EventHandlers[eventName] += new Action<string, string, string>((j1, j2, j3) =>
            {
                action(JsonConvert.DeserializeObject<T1>(j1), JsonConvert.DeserializeObject<T2>(j2), JsonConvert.DeserializeObject<T3>(j3));
            });
        }

        public void AddEventHandler(string name, Action action) => EventHandlers[name] += action;
        public void AddEventHandler<T1>(string name, Action<T1> action) => EventHandlers[name] += action;
        public void AddEventHandler<T1, T2>(string name, Action<T1, T2> action) => EventHandlers[name] += action;
        public void AddEventHandler<T1, T2, T3>(string name, Action<T1, T2, T3> action) => EventHandlers[name] += action;

        private void RegisterEvents()
        {
            HandleJsonEvent<User>("igi:user:load", u => User.Load(this, u));

            EventHandlers["igi:character:new"] += new Action<string>(NewCharacter);
            HandleJsonEvent<Character>("igi:character:load", c => Character.Load(this, c));

            EventHandlers["igi:user:gps"] += new Action(UserGps);
        }



        private void NewCharacter(string charJson)
        {
            User.Character = Character.Load(charJson);
        }

        public void UserGps()
        {
            Debug.WriteLine($"UserGps Called: {LocalPlayer.Character.Position}");
        }
    }
}
