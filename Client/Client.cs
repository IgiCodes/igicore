using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using IgiCore.Client.Models;
using IgiCore.Core.Models.Appearance;
using Newtonsoft.Json;
using Citizen = CitizenFX.Core.Player;

namespace IgiCore.Client
{
    public class Client : BaseScript
    {
        private static readonly int AutoSaveInterval = (int)TimeSpan.FromSeconds(10).TotalMilliseconds;
        private readonly object autosaveLock = new object();

        private User user;
        private Character character;

        public Citizen Citizen => LocalPlayer;

        public Client()
        {
            Tick += ClientTick;
            Tick += ClientTickAutoSave;

            RegisterEvents();

            TriggerServerEvent("igi:user:load");
        }

        protected void HandleEvent(string eventName, Action action)
        {
            EventHandlers[eventName] += action;
        }

        protected void HandleJsonEvent<T>(string eventName, Action<T> action)
        {
            EventHandlers[eventName] += new Action<string>(json =>
            {
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

        private void RegisterEvents()
        {
            HandleJsonEvent<User>("igi:user:load", new Action<User>(u => this.user = u ));

            EventHandlers["igi:character:new"] += new Action<string>(NewCharacter);
            HandleJsonEvent<Character>("igi:character:load", LoadCharacter);

            EventHandlers["igi:user:gps"] += new Action(UserGps);

        }

        private async Task ClientTick()
        {
            CheckAlive();

            await Delay(1);
        }

        private async Task ClientTickAutoSave()
        {
            if (character == null)
            {
                await Delay(AutoSaveInterval);
                return;
            }

            lock (autosaveLock)
            {
                Debug.WriteLine("=========== Autosaving Character ===========");

                character.Position = LocalPlayer.Character.Position;
                character.Save();
            }

            await Delay(AutoSaveInterval);
        }

        private void CheckAlive()
        {
            if (character == null) return;
            if (character.Alive) return;

            character.Respawn(LocalPlayer);
        }

        private void LoadUser(User user)
        {
            this.user = user;
        }

        private void NewCharacter(string charJson)
        {
            character = Character.Load(charJson);
        }

        private void LoadCharacter(Character character)
        {
            Debug.WriteLine($"[CLIENT]: Loading character: {character}");

            lock (autosaveLock)
            {
                this.character = character;

                EventHandlers["igi:character:component:set"] += new Action<ComponentTypes, int, int>(character.SetComponent);
                EventHandlers["igi:character:prop:set"] += new Action<PropTypes, int, int>(character.SetProp);

                Citizen.Character.Position = new Vector3 { X = character.PosX, Y = character.PosY, Z = character.PosZ };
            }
        }

        public void UserGps()
        {
            Debug.WriteLine($"UserGps Called: {LocalPlayer.Character.Position}");
        }
    }
}
