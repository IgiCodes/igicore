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
        private static readonly int AutoSaveInterval = (int)TimeSpan.FromSeconds(10).TotalMilliseconds;
        private readonly object autosaveLock = new object();

        public Character Character;
        public User User;

        public Citizen Citizen => LocalPlayer;

        public Client()
        {
            Tick += ClientTick;
            Tick += ClientTickAutoSave;

            RegisterEvents();
        }

        protected void HandleEvent(string eventName, Action action)
        {
            EventHandlers[eventName] += action;
        }

        protected void HandleEvent<T>(string eventName, Action<T> action)
        {
            EventHandlers[eventName] += new Action<string>(json =>
            {
                action(JsonConvert.DeserializeObject<T>(json));
            });
        }

        protected void HandleEvent<T1, T2>(string eventName, Action<T1, T2> action)
        {
            EventHandlers[eventName] += new Action<string, string>((j1, j2) =>
            {
                action(JsonConvert.DeserializeObject<T1>(j1), JsonConvert.DeserializeObject<T2>(j2));
            });
        }

        protected void HandleEvent<T1, T2, T3>(string eventName, Action<T1, T2, T3> action)
        {
            EventHandlers[eventName] += new Action<string, string, string>((j1, j2, j3) =>
            {
                action(JsonConvert.DeserializeObject<T1>(j1), JsonConvert.DeserializeObject<T2>(j2), JsonConvert.DeserializeObject<T3>(j3));
            });
        }

        private void RegisterEvents()
        {
            EventHandlers["igi:character:new"] += new Action<string>(NewCharacter);
            //EventHandlers["igi:character:load"] += new Action<string>(LoadCharacter);
            HandleEvent<Character>("igi:character:load", LoadCharacter);

            EventHandlers["igi:user:gps"] += new Action(UserGps);
        }

        private async Task ClientTick()
        {
            CheckAlive();

            await Delay(1);
        }

        private async Task ClientTickAutoSave()
        {
            if (Character == null)
            {
                await Delay(AutoSaveInterval);
                return;
            }

            lock (autosaveLock)
            {
                Debug.WriteLine("=========== Autosaving Character ===========");

                Character.Position = LocalPlayer.Character.Position;
                Character.Save();
            }

            await Delay(AutoSaveInterval);
        }

        private void CheckAlive()
        {
            if (Character == null) return;
            if (Character.Alive) return;

            Character.Respawn(LocalPlayer);
        }

        private void NewCharacter(string charJson)
        {
            Character = Character.Load(charJson);
        }

        private void LoadCharacter(Character character)
        {
            Debug.WriteLine($"[CLIENT]: Loading character: {character}");

            lock (autosaveLock)
            {
                Character = character;
                LocalPlayer.Character.Position = new Vector3 { X = Character.PosX, Y = Character.PosY, Z = Character.PosZ };
            }
        }

        public void UserGps()
        {
            Debug.WriteLine($"UserGps Called: {LocalPlayer.Character.Position}");
        }
    }
}
