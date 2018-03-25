using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using IgiCore.Core.Extensions;
using IgiCore.Core.Models.Appearance;
using IgiCore.Core.Models.Player;
using Style = IgiCore.Core.Models.Appearance.Style;
using Prop = IgiCore.Core.Models.Appearance.Prop;
using Newtonsoft.Json;

namespace IgiCore.Client.Models
{
    public class Character : ICharacter, IDisposable
    {
        protected static readonly int AutoSaveInterval = (int)TimeSpan.FromSeconds(10).TotalMilliseconds;
        protected readonly object AutosaveLock = new object();

        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Alive { get; set; }
        public DateTime LastPlayed { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }
        public Style Style { get; set; }

        [JsonIgnore]
        public Vector3 Position
        {
            get => new Vector3(this.PosX, this.PosY, this.PosZ);
            set
            {
                this.PosX = value.X;
                this.PosY = value.Y;
                this.PosZ = value.Z;
            }
        }

        [JsonIgnore]
        protected Client Client;

        // Pretend ctor
        public void Initialize(Client client)
        {
            // Store client reference
            this.Client = client;

            this.Client.HandleEvent<int, int, int>("igi:character:component:set", SetComponent);
            this.Client.HandleEvent<int, int, int>("igi:character:prop:set", SetProp);

            this.Client.Tick += OnTick;
        }

        protected async Task OnTick()
        {
            Client.Log("Autosaving");
            // Autosave
            lock (this.AutosaveLock)
            {
                this.Position = this.Client.LocalPlayer.Character.Position;
                this.LastPlayed = DateTime.UtcNow;

                BaseScript.TriggerServerEvent("igi:character:save", JsonConvert.SerializeObject(this));
            }

            await BaseScript.Delay(AutoSaveInterval);
        }

        // Sync this character to LocalPlayer
        public void Render()
        {
            this.Client.LocalPlayer.Character.Position = this.Position;

            ApplyStyle();
        }

        public void SetComponent(int type, int index, int texture)
        {
            PedComponents componentType = (PedComponents)type;
            this.Client.LocalPlayer.Character.Style[componentType].Index = index;
            this.Client.LocalPlayer.Character.Style[componentType].TextureIndex = texture;

            this.Style = ConvertStyle(this.Client.LocalPlayer.Character.Style, this.Style.Id);
        }

        public void SetProp(int type, int index, int texture)
        {
            PedProps propType = (PedProps)type;
            this.Client.LocalPlayer.Character.Style[propType].Index = index;
            this.Client.LocalPlayer.Character.Style[propType].TextureIndex = texture;

            this.Style = ConvertStyle(this.Client.LocalPlayer.Character.Style, this.Style.Id);
        }

        public void Dispose()
        {
            this.Client.Tick -= OnTick;
        }

        public override string ToString()
        {
            return $"Character [{this.Id}]: {this.Name}, {(this.Alive ? "Alive" : "Dead")}";
        }

        protected static Style ConvertStyle(CitizenFX.Core.Style citStyle, Guid? styleId = null) => new Style
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
            Unknown9 = new Prop { Type = PropTypes.Unknown9, Index = citStyle[PedProps.Unknown9].Index, Texture = citStyle[PedProps.Unknown9].TextureIndex }
        };

        protected void ApplyStyle()
        {
            this.Client.LocalPlayer.Character.Style[PedComponents.Face].SetVariation(this.Style.Face.Index, this.Style.Face.Texture);
            this.Client.LocalPlayer.Character.Style[PedComponents.Head].SetVariation(this.Style.Head.Index, this.Style.Head.Texture);
            this.Client.LocalPlayer.Character.Style[PedComponents.Hair].SetVariation(this.Style.Hair.Index, this.Style.Hair.Texture);
            this.Client.LocalPlayer.Character.Style[PedComponents.Torso].SetVariation(this.Style.Torso.Index, this.Style.Torso.Texture);
            this.Client.LocalPlayer.Character.Style[PedComponents.Legs].SetVariation(this.Style.Legs.Index, this.Style.Legs.Texture);
            this.Client.LocalPlayer.Character.Style[PedComponents.Hands].SetVariation(this.Style.Hands.Index, this.Style.Hands.Texture);
            this.Client.LocalPlayer.Character.Style[PedComponents.Shoes].SetVariation(this.Style.Shoes.Index, this.Style.Shoes.Texture);
            this.Client.LocalPlayer.Character.Style[PedComponents.Special1].SetVariation(this.Style.Special1.Index, this.Style.Special1.Texture);
            this.Client.LocalPlayer.Character.Style[PedComponents.Special2].SetVariation(this.Style.Special2.Index, this.Style.Special2.Texture);
            this.Client.LocalPlayer.Character.Style[PedComponents.Special3].SetVariation(this.Style.Special3.Index, this.Style.Special3.Texture);
            this.Client.LocalPlayer.Character.Style[PedComponents.Textures].SetVariation(this.Style.Textures.Index, this.Style.Textures.Texture);
            this.Client.LocalPlayer.Character.Style[PedComponents.Torso2].SetVariation(this.Style.Torso2.Index, this.Style.Torso2.Texture);

            this.Client.LocalPlayer.Character.Style[PedProps.Hats].SetVariation(this.Style.Hat.Index, this.Style.Hat.Texture);
            this.Client.LocalPlayer.Character.Style[PedProps.Glasses].SetVariation(this.Style.Glasses.Index, this.Style.Glasses.Texture);
            this.Client.LocalPlayer.Character.Style[PedProps.EarPieces].SetVariation(this.Style.EarPiece.Index, this.Style.EarPiece.Texture);
            this.Client.LocalPlayer.Character.Style[PedProps.Unknown3].SetVariation(this.Style.Unknown3.Index, this.Style.Unknown3.Texture);
            this.Client.LocalPlayer.Character.Style[PedProps.Unknown4].SetVariation(this.Style.Unknown4.Index, this.Style.Unknown4.Texture);
            this.Client.LocalPlayer.Character.Style[PedProps.Unknown5].SetVariation(this.Style.Unknown5.Index, this.Style.Unknown5.Texture);
            this.Client.LocalPlayer.Character.Style[PedProps.Watches].SetVariation(this.Style.Watch.Index, this.Style.Watch.Texture);
            this.Client.LocalPlayer.Character.Style[PedProps.Wristbands].SetVariation(this.Style.Wristband.Index, this.Style.Wristband.Texture);
            this.Client.LocalPlayer.Character.Style[PedProps.Unknown8].SetVariation(this.Style.Unknown8.Index, this.Style.Unknown8.Texture);
            this.Client.LocalPlayer.Character.Style[PedProps.Unknown9].SetVariation(this.Style.Unknown9.Index, this.Style.Unknown9.Texture);
        }
    }
}
