using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using IgiCore.Core.Extensions;
using IgiCore.Core.Models;
using IgiCore.Core.Models.Appearance;
using Style = IgiCore.Core.Models.Appearance.Style;
using Prop = IgiCore.Core.Models.Appearance.Prop;
using Newtonsoft.Json;
using Citizen = CitizenFX.Core.Player;

namespace IgiCore.Client.Models
{
    public class Character : ICharacter
    {
        private static readonly int AutoSaveInterval = (int)TimeSpan.FromSeconds(10).TotalMilliseconds;
        private static readonly object autosaveLock = new object();
        private event EventHandler<CharacterReadyEventArgs> Loaded;

        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Alive { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }

        public Style Style { get; set; }

        [JsonIgnore]
        public Citizen Citizen { get; set; }

        [JsonIgnore]
        public Vector3 Position
        {
            get => new Vector3(PosX, PosY, PosZ);
            set
            {
                PosX = value.X;
                PosY = value.Y;
                PosZ = value.Z;
            }
        }

        public Character()
        {
            Loaded += CharacterReady;
        }

        public static Character Load(string json)
        {
            return JsonConvert.DeserializeObject<Character>(json);
        }

        private void CheckAlive()
        {
            if (Alive) return;

            Respawn();
        }

        public void Respawn()
        {
            Citizen.Character.Position = new Vector3 { X = -1038.121f, Y = -2738.279f, Z = 20.16929f };

            Position = Citizen.Character.Position;
            Alive = true;

            Save();
        }

        public static void Load(Client client, Character charToLoad)
        {
            lock (Character.autosaveLock)
            {
                if (client.User.Character != null) client.User.Character.Unload(client);
                client.User.Character = charToLoad;
                client.User.Character.Citizen = client.Citizen;
                client.Citizen.ChangeModel(new Model(PedHash.FreemodeMale01));
                client.Citizen.Character.Position = charToLoad.Position;
                client.User.Character.ApplyStyle(charToLoad.Style);

                client.User.Character.Loaded?.Invoke(client.User.Character, new CharacterReadyEventArgs(client));
            }
        }

        public void Unload(Client client)
        {
            client.CharTick -= CharAutoSave;
        }

        public void CharacterReady(object sender, CharacterReadyEventArgs e)
        {
            e.Client.AddEventHandler<int, int, int>("igi:character:component:set", e.Client.User.Character.SetComponent);
            e.Client.AddEventHandler<int, int, int>("igi:character:prop:set", e.Client.User.Character.SetProp);

            e.Client.CharTick += CharAutoSave;

        }

        public class CharacterReadyEventArgs : EventArgs
        {
            public Client Client { get; protected set; }

            public CharacterReadyEventArgs(Client client)
            {
                Client = client;
            }
        }

        private async Task CharAutoSave()
        {
            lock (Character.autosaveLock)
            {
                Position = Citizen.Character.Position;
                Save();
            }

            await BaseScript.Delay(AutoSaveInterval);
        }

        public void Save() => BaseScript.TriggerServerEvent("igi:character:save", JsonConvert.SerializeObject(this));

        public void SetComponent(int type, int index, int texture)
        {
            PedComponents componentType = (PedComponents)type;
            Citizen.Character.Style[componentType].Index = index;
            Citizen.Character.Style[componentType].TextureIndex = texture;
            Style = ConvertStyle(Citizen.Character.Style, Style.Id);
        }

        public void SetProp(int type, int index, int texture)
        {
            PedProps propType = (PedProps)type;
            Citizen.Character.Style[propType].Index = index;
            Citizen.Character.Style[propType].TextureIndex = texture;
            Style = ConvertStyle(Citizen.Character.Style, Style.Id);
        }

        public override string ToString()
        {
            return $"Character [{Id}]: {Name}, {(Alive ? "Alive" : "Dead")}";
        }

        public static Style ConvertStyle(CitizenFX.Core.Style citStyle, Guid? styleId = null) => new Style
        {
            Id = styleId ?? GuidGenerator.GenerateTimeBasedGuid(),
            Face = new Component { Type = ComponentTypes.Face, Index = citStyle[PedComponents.Face].Index, Texture = citStyle[PedComponents.Face].TextureIndex },
            Head = new Component { Type = ComponentTypes.Head, Index = citStyle[PedComponents.Head].Index, Texture = citStyle[PedComponents.Head].TextureIndex },
            Hair = new Component { Type = ComponentTypes.Hair, Index = citStyle[PedComponents.Hair].Index, Texture = citStyle[PedComponents.Hair].TextureIndex },
            Torso = new Component { Type = ComponentTypes.Torso, Index = citStyle[PedComponents.Torso].Index, Texture = citStyle[PedComponents.Torso].TextureIndex },
            Legs = new Component { Type = ComponentTypes.Legs, Index = citStyle[PedComponents.Legs].Index, Texture = citStyle[PedComponents.Legs].TextureIndex },
            Hands = new Component { Type = ComponentTypes.Hands, Index = citStyle[PedComponents.Hands].Index, Texture = citStyle[PedComponents.Hands].TextureIndex },
            Shoes = new Component { Type = ComponentTypes.Shoes, Index = citStyle[PedComponents.Shoes].Index, Texture = citStyle[PedComponents.Shoes].TextureIndex },
            Special1 = new Component { Type = ComponentTypes.Special1, Index = citStyle[PedComponents.Special1].Index, Texture = citStyle[PedComponents.Special1].TextureIndex },
            Special2 = new Component { Type = ComponentTypes.Special2, Index = citStyle[PedComponents.Special2].Index, Texture = citStyle[PedComponents.Special2].TextureIndex },
            Special3 = new Component { Type = ComponentTypes.Special3, Index = citStyle[PedComponents.Special3].Index, Texture = citStyle[PedComponents.Special3].TextureIndex },
            Textures = new Component { Type = ComponentTypes.Textures, Index = citStyle[PedComponents.Textures].Index, Texture = citStyle[PedComponents.Textures].TextureIndex },
            Torso2 = new Component { Type = ComponentTypes.Torso2, Index = citStyle[PedComponents.Torso2].Index, Texture = citStyle[PedComponents.Torso2].TextureIndex },

            Hat = new Prop { Type = PropTypes.Hats, Index = citStyle[PedProps.Hats].Index, Texture = citStyle[PedProps.Hats].TextureIndex },
            Glasses = new Prop { Type = PropTypes.Glasses, Index = citStyle[PedProps.Glasses].Index, Texture = citStyle[PedProps.Glasses].TextureIndex },
            EarPiece = new Prop { Type = PropTypes.EarPieces, Index = citStyle[PedProps.EarPieces].Index, Texture = citStyle[PedProps.EarPieces].TextureIndex },
            Unknown3 = new Prop { Type = PropTypes.Unknown3, Index = citStyle[PedProps.Unknown3].Index, Texture = citStyle[PedProps.Unknown3].TextureIndex },
            Unknown4 = new Prop { Type = PropTypes.Unknown4, Index = citStyle[PedProps.Unknown4].Index, Texture = citStyle[PedProps.Unknown4].TextureIndex },
            Unknown5 = new Prop { Type = PropTypes.Unknown5, Index = citStyle[PedProps.Unknown5].Index, Texture = citStyle[PedProps.Unknown5].TextureIndex },
            Watch = new Prop { Type = PropTypes.Watches, Index = citStyle[PedProps.Watches].Index, Texture = citStyle[PedProps.Watches].TextureIndex },
            Wristband = new Prop { Type = PropTypes.Wristbands, Index = citStyle[PedProps.Wristbands].Index, Texture = citStyle[PedProps.Wristbands].TextureIndex },
            Unknown8 = new Prop { Type = PropTypes.Unknown8, Index = citStyle[PedProps.Unknown8].Index, Texture = citStyle[PedProps.Unknown8].TextureIndex },
            Unknown9 = new Prop { Type = PropTypes.Unknown9, Index = citStyle[PedProps.Unknown9].Index, Texture = citStyle[PedProps.Unknown9].TextureIndex },
        };

        public void ApplyStyle(Style style)
        {
            Citizen.Character.Style[PedComponents.Face].SetVariation(style.Face.Index, style.Face.Texture);
            Citizen.Character.Style[PedComponents.Head].SetVariation(style.Head.Index, style.Face.Texture);
            Citizen.Character.Style[PedComponents.Hair].SetVariation(style.Hair.Index, style.Face.Texture);
            Citizen.Character.Style[PedComponents.Torso].SetVariation(style.Torso.Index, style.Face.Texture);
            Citizen.Character.Style[PedComponents.Legs].SetVariation(style.Legs.Index, style.Face.Texture);
            Citizen.Character.Style[PedComponents.Hands].SetVariation(style.Hands.Index, style.Face.Texture);
            Citizen.Character.Style[PedComponents.Shoes].SetVariation(style.Shoes.Index, style.Face.Texture);
            Citizen.Character.Style[PedComponents.Special1].SetVariation(style.Special1.Index, style.Face.Texture);
            Citizen.Character.Style[PedComponents.Special2].SetVariation(style.Special2.Index, style.Face.Texture);
            Citizen.Character.Style[PedComponents.Special3].SetVariation(style.Special3.Index, style.Face.Texture);
            Citizen.Character.Style[PedComponents.Textures].SetVariation(style.Textures.Index, style.Face.Texture);
            Citizen.Character.Style[PedComponents.Torso2].SetVariation(style.Torso2.Index, style.Face.Texture);

            Citizen.Character.Style[PedProps.Hats].SetVariation(style.Hat.Index, style.Hat.Texture);
            Citizen.Character.Style[PedProps.Glasses].SetVariation(style.Glasses.Index, style.Glasses.Texture);
            Citizen.Character.Style[PedProps.EarPieces].SetVariation(style.EarPiece.Index, style.EarPiece.Texture);
            Citizen.Character.Style[PedProps.Unknown3].SetVariation(style.Unknown3.Index, style.Unknown3.Texture);
            Citizen.Character.Style[PedProps.Unknown4].SetVariation(style.Unknown4.Index, style.Unknown4.Texture);
            Citizen.Character.Style[PedProps.Unknown5].SetVariation(style.Unknown5.Index, style.Unknown5.Texture);
            Citizen.Character.Style[PedProps.Watches].SetVariation(style.Watch.Index, style.Watch.Texture);
            Citizen.Character.Style[PedProps.Wristbands].SetVariation(style.Wristband.Index, style.Wristband.Texture);
            Citizen.Character.Style[PedProps.Unknown8].SetVariation(style.Unknown8.Index, style.Unknown8.Texture);
            Citizen.Character.Style[PedProps.Unknown9].SetVariation(style.Unknown9.Index, style.Unknown9.Texture);
        }
    }
}
