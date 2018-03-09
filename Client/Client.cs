using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using IgiCore.Client.Models;
using Citizen = CitizenFX.Core.Player;

namespace IgiCore.Client
{
    public class Client : BaseScript
    {
        private static readonly int AutoSaveInterval = (int)TimeSpan.FromSeconds(10).TotalMilliseconds;

        public Character Character;
        public User User;

        public Citizen Citizen => LocalPlayer;

        public Client()
        {
            Tick += ClientTick;
            Tick += ClientTickAutoSave;

            RegisterEvents();
        }

        private void RegisterEvents()
        {
            EventHandlers["igi:character:new"] += new Action<string>(NewCharacter);
            EventHandlers["igi:character:load"] += new Action<string>(LoadCharacter);

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

            Debug.WriteLine("=========== Autosaving Character ===========");

            Character.PosX = LocalPlayer.Character.Position.X;
            Character.PosY = LocalPlayer.Character.Position.Y;
            Character.PosZ = LocalPlayer.Character.Position.Z;
            Character.Save();

            await Delay(AutoSaveInterval);
        }

        private void CheckAlive()
        {
            if (Character == null) return;

            if (!Character.Alive) Character.Respawn(LocalPlayer);
        }

        private void NewCharacter(string charJson)
        {
            Character = Character.Load(charJson);
        }

        private void LoadCharacter(string charJson)
        {
            Character charToLoad = Character.Load(charJson);

            if (charToLoad != null)
            {
                Debug.WriteLine($"Loading Character {Character.Name}");

                Character = charToLoad;
                LocalPlayer.Character.Position = new Vector3 { X = Character.PosX, Y = Character.PosY, Z = Character.PosZ };
            }
            else
            {
                Debug.WriteLine("Invalid Character ID Passed");
            }
        }

        public void UserGps()
        {
            Debug.WriteLine("UserGPS Called");
            Vector3 pos = LocalPlayer.Character.Position;
            Debug.WriteLine(pos.ToString());
        }
    }
}
