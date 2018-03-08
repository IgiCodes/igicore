using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using Citizen = CitizenFX.Core.Player;
using IgiCore.Client;
using IgiCore.Core.Models;
using Newtonsoft.Json;
using System;

namespace IgiCore.Client.Models
{
    public class Character : ICharacter
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public IUser User { get; set; }
        public string Name { get; set; }
        public bool Alive { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }

        public object Lock = new object();

        public static Character Load(string json) => JsonConvert.DeserializeObject<Character>(json);

        public void Respawn(Citizen citizen)
        {
            Vector3 spawnLocation = new Vector3 { X = -1038.121f, Y = -2738.279f, Z = 20.16929f };
            citizen.Character.Position = spawnLocation;

            this.PosX = citizen.Character.Position.X;
            this.PosY = citizen.Character.Position.Y;
            this.PosZ = citizen.Character.Position.Z;
            Alive = true;
            Save();

            
        }

        public void Save()
        {
            lock (Lock)
            {
                BaseScript.TriggerServerEvent("igi:character:save", JsonConvert.SerializeObject(this));
            };
        }
    }
}
