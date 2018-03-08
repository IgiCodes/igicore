using CitizenFX.Core;
using Citizen = CitizenFX.Core.Player;
using IgiCore.Client.Models;
using IgiCore.Core.Models;
using System;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IgiCore.Client
{

    public partial class Client : BaseScript
    {
        //private static int autoSaveInterval = (int)TimeSpan.FromMinutes(5).TotalMilliseconds;
        private static int autoSaveInterval = (int)TimeSpan.FromSeconds(10).TotalMilliseconds;

        public User user;
        public Character character;
        public Citizen Citizen => this.LocalPlayer;

        private object autosaveLock = new object();
        //public AutoResetEvent autosave = new AutoResetEvent(false);

        public Client()
        {
            Tick += PlayerClient_Tick;
            Tick += PlayerClient_autoSave;

            RegisterEvents();
        }

        private void RegisterEvents()
        {
            EventHandlers["igi:character:new"] += new Action<string>(NewCharacter);
            EventHandlers["igi:character:load"] += new Action<string>(LoadCharacter);

            EventHandlers["igi:user:gps"] += new Action(UserGPS);
        }

        public void UserGPS()
        {
            Debug.WriteLine("UserGPS Called");
            Vector3 pos = LocalPlayer.Character.Position;
            Debug.WriteLine(pos.ToString());
        }

        private async Task PlayerClient_Tick()
        {
            CheckAlive();

            await Delay(1);
        }

        private async Task PlayerClient_autoSave()
        {
            //autosave.WaitOne();

            if (character == null)
            {
                await Delay(autoSaveInterval);
                return;
            }
            Debug.WriteLine("=========== Autosaving Character ===========");
            character.PosX = LocalPlayer.Character.Position.X;
            character.PosY = LocalPlayer.Character.Position.Y;
            character.PosZ = LocalPlayer.Character.Position.Z;
            character.Save();
            //autosave.Reset();
            await Delay(autoSaveInterval);
        }

        private void CheckAlive()
        {
            if (character == null) return;
            if (!character.Alive)
            {
                //autosave.WaitOne();
                character.Respawn(LocalPlayer);
                //autosave.Reset();
            }

        }

        private void NewCharacter(string charJson)
        {
            character = Character.Load(charJson);
        }

        private void LoadCharacter(string charJson)
        {
            //autosave.WaitOne();
            Character charToLoad = Character.Load(charJson);
            if (charToLoad != null)
            {
                Debug.WriteLine($"Loading Character {character.Name}");

                character = charToLoad;
                LocalPlayer.Character.Position = new Vector3 { X = character.PosX, Y = character.PosY, Z = character.PosZ };
            }
            else
            {
                Debug.WriteLine("Invalid Character ID Passed");
            }
            //autosave.Reset();
        }

    }

}