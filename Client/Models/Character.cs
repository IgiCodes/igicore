using System;
using CitizenFX.Core;
using IgiCore.Core.Models;
using IgiCore.Core.Models.Appearance;
using Newtonsoft.Json;
using Citizen = CitizenFX.Core.Player;

namespace IgiCore.Client.Models
{
    public class Character : ICharacter
    {
        private readonly object saveLock = new object();

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public IUser User { get; set; }
        public string Name { get; set; }
        public bool Alive { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }

        public Core.Models.Appearance.Style Style { get; set; }

        [JsonIgnore]
        public Citizen Citizen { get; set; }
        [JsonIgnore]
        public Vector3 Position {
            get => new Vector3(PosX, PosY, PosZ);
            set
            {
                PosX = value.X;
                PosY = value.Y;
                PosZ = value.Z;
            }
        }

        public static Character Load(string json)
        {
            return JsonConvert.DeserializeObject<Character>(json);
        }

        public void Respawn(Citizen citizen)
        {
            citizen.Character.Position =  new Vector3 { X = -1038.121f, Y = -2738.279f, Z = 20.16929f };

            Position = citizen.Character.Position;
            Alive = true;

            Save();
        }

        public void Save()
        {
            lock (saveLock)
            {
                BaseScript.TriggerServerEvent("igi:character:save", JsonConvert.SerializeObject(this));
            }
        }

        public void SetComponent(ComponentTypes type, int index, int texture)
        {
            Debug.WriteLine($"SetComponent: Type = {type}  Index = {index}  texture = {texture}");
        }

        public void SetProp(PropTypes type, int index, int texture)
        {
            Debug.WriteLine($"SetProp: Type = {type}  Index = {index}  texture = {texture}");
        }

        public override string ToString()
        {
            return $"Character [{Id}]: {Name}, {(Alive ? "Alive" : "Dead")}";
        }
    }
}
