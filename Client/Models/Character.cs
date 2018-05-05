using System;
using System.Collections.Generic;
using CitizenFX.Core;
using IgiCore.Client.Rpc;
using IgiCore.Core.Extensions;
using IgiCore.Core.Models.Appearance;
using IgiCore.Core.Models.Inventories.Characters;
using IgiCore.Core.Models.Player;
using Newtonsoft.Json;
using Prop = IgiCore.Core.Models.Appearance.Prop;
using Style = IgiCore.Core.Models.Appearance.Style;

namespace IgiCore.Client.Models
{
	public class Character : ICharacter, IDisposable
	{
		protected readonly object SaveLock = new object();

		public Guid Id { get; set; }
		public string Forename { get; set; }
		public string Middlename { get; set; }
		public string Surname { get; set; }
		public DateTime DateOfBirth { get; set; }
		public short Gender { get; set; }
		public bool Alive { get; set; }
		public int Health { get; set; }
		public int Armor { get; set; }
		public string Ssn { get; set; }
		public float PosX { get; set; }
		public float PosY { get; set; }
		public float PosZ { get; set; }
		public string Model { get; set; }
		public string WalkingStyle { get; set; }
		public Inventory Inventory { get; set; }
		public Style Style { get; set; }
		public DateTime LastPlayed { get; set; }
		public DateTime Created { get; set; }
		public List<Skill> Skills { get; set; }

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
		public string FullName => $"{this.Forename} {this.Middlename} {this.Surname}".Replace("  ", " ");

		public void Initialize()
		{
			Server.On<int, int, int>("igi:character:component:set", SetComponent);
			Server.On<int, int, int>("igi:character:prop:set", SetProp);
		}

		public void Save()
		{
			lock (this.SaveLock)
			{
				this.Position = Game.Player.Character.Position;

				Server.Trigger("igi:character:save", this);
			}
		}

		// Sync this character to LocalPlayer
		public void Render()
		{
			Game.Player.Character.Position = this.Position;

			Game.Player.Character.Style[PedComponents.Face].SetVariation(this.Style.Face.Index, this.Style.Face.Texture);
			Game.Player.Character.Style[PedComponents.Head].SetVariation(this.Style.Head.Index, this.Style.Head.Texture);
			Game.Player.Character.Style[PedComponents.Hair].SetVariation(this.Style.Hair.Index, this.Style.Hair.Texture);
			Game.Player.Character.Style[PedComponents.Torso].SetVariation(this.Style.Torso.Index, this.Style.Torso.Texture);
			Game.Player.Character.Style[PedComponents.Legs].SetVariation(this.Style.Legs.Index, this.Style.Legs.Texture);
			Game.Player.Character.Style[PedComponents.Hands].SetVariation(this.Style.Hands.Index, this.Style.Hands.Texture);
			Game.Player.Character.Style[PedComponents.Shoes].SetVariation(this.Style.Shoes.Index, this.Style.Shoes.Texture);
			Game.Player.Character.Style[PedComponents.Special1].SetVariation(this.Style.Special1.Index, this.Style.Special1.Texture);
			Game.Player.Character.Style[PedComponents.Special2].SetVariation(this.Style.Special2.Index, this.Style.Special2.Texture);
			Game.Player.Character.Style[PedComponents.Special3].SetVariation(this.Style.Special3.Index, this.Style.Special3.Texture);
			Game.Player.Character.Style[PedComponents.Textures].SetVariation(this.Style.Textures.Index, this.Style.Textures.Texture);
			Game.Player.Character.Style[PedComponents.Torso2].SetVariation(this.Style.Torso2.Index, this.Style.Torso2.Texture);

			Game.Player.Character.Style[PedProps.Hats].SetVariation(this.Style.Hat.Index, this.Style.Hat.Texture);
			Game.Player.Character.Style[PedProps.Glasses].SetVariation(this.Style.Glasses.Index, this.Style.Glasses.Texture);
			Game.Player.Character.Style[PedProps.EarPieces].SetVariation(this.Style.EarPiece.Index, this.Style.EarPiece.Texture);
			Game.Player.Character.Style[PedProps.Unknown3].SetVariation(this.Style.Unknown3.Index, this.Style.Unknown3.Texture);
			Game.Player.Character.Style[PedProps.Unknown4].SetVariation(this.Style.Unknown4.Index, this.Style.Unknown4.Texture);
			Game.Player.Character.Style[PedProps.Unknown5].SetVariation(this.Style.Unknown5.Index, this.Style.Unknown5.Texture);
			Game.Player.Character.Style[PedProps.Watches].SetVariation(this.Style.Watch.Index, this.Style.Watch.Texture);
			Game.Player.Character.Style[PedProps.Wristbands].SetVariation(this.Style.Wristband.Index, this.Style.Wristband.Texture);
			Game.Player.Character.Style[PedProps.Unknown8].SetVariation(this.Style.Unknown8.Index, this.Style.Unknown8.Texture);
			Game.Player.Character.Style[PedProps.Unknown9].SetVariation(this.Style.Unknown9.Index, this.Style.Unknown9.Texture);
		}

		public void SetComponent(int type, int index, int texture)
		{
			PedComponents componentType = (PedComponents)type;
			Game.Player.Character.Style[componentType].Index = index;
			Game.Player.Character.Style[componentType].TextureIndex = texture;

			this.Style = ConvertStyle(Game.Player.Character.Style, this.Style.Id);
		}

		public void SetProp(int type, int index, int texture)
		{
			PedProps propType = (PedProps)type;
			Game.Player.Character.Style[propType].Index = index;
			Game.Player.Character.Style[propType].TextureIndex = texture;

			this.Style = ConvertStyle(Game.Player.Character.Style, this.Style.Id);
		}

		public override string ToString() => $"Character [{this.Id}]: {this.FullName}, {this.Position}";

		public void Dispose()
		{
			
		}

		protected static Style ConvertStyle(CitizenFX.Core.Style style, Guid? id = null)
		{
			return new Style
			{
				Id = id ?? GuidGenerator.GenerateTimeBasedGuid(),
				Face = new Component
				{
					Type = ComponentTypes.Face,
					Index = style[PedComponents.Face].Index,
					Texture = style[PedComponents.Face].TextureIndex
				},
				Head = new Component
				{
					Type = ComponentTypes.Head,
					Index = style[PedComponents.Head].Index,
					Texture = style[PedComponents.Head].TextureIndex
				},
				Hair = new Component
				{
					Type = ComponentTypes.Hair,
					Index = style[PedComponents.Hair].Index,
					Texture = style[PedComponents.Hair].TextureIndex
				},
				Torso = new Component
				{
					Type = ComponentTypes.Torso,
					Index = style[PedComponents.Torso].Index,
					Texture = style[PedComponents.Torso].TextureIndex
				},
				Legs = new Component
				{
					Type = ComponentTypes.Legs,
					Index = style[PedComponents.Legs].Index,
					Texture = style[PedComponents.Legs].TextureIndex
				},
				Hands = new Component
				{
					Type = ComponentTypes.Hands,
					Index = style[PedComponents.Hands].Index,
					Texture = style[PedComponents.Hands].TextureIndex
				},
				Shoes = new Component
				{
					Type = ComponentTypes.Shoes,
					Index = style[PedComponents.Shoes].Index,
					Texture = style[PedComponents.Shoes].TextureIndex
				},
				Special1 = new Component
				{
					Type = ComponentTypes.Special1,
					Index = style[PedComponents.Special1].Index,
					Texture = style[PedComponents.Special1].TextureIndex
				},
				Special2 = new Component
				{
					Type = ComponentTypes.Special2,
					Index = style[PedComponents.Special2].Index,
					Texture = style[PedComponents.Special2].TextureIndex
				},
				Special3 = new Component
				{
					Type = ComponentTypes.Special3,
					Index = style[PedComponents.Special3].Index,
					Texture = style[PedComponents.Special3].TextureIndex
				},
				Textures = new Component
				{
					Type = ComponentTypes.Textures,
					Index = style[PedComponents.Textures].Index,
					Texture = style[PedComponents.Textures].TextureIndex
				},
				Torso2 = new Component
				{
					Type = ComponentTypes.Torso2,
					Index = style[PedComponents.Torso2].Index,
					Texture = style[PedComponents.Torso2].TextureIndex
				},

				Hat = new Prop
				{
					Type = PropTypes.Hats,
					Index = style[PedProps.Hats].Index,
					Texture = style[PedProps.Hats].TextureIndex
				},
				Glasses = new Prop
				{
					Type = PropTypes.Glasses,
					Index = style[PedProps.Glasses].Index,
					Texture = style[PedProps.Glasses].TextureIndex
				},
				EarPiece = new Prop
				{
					Type = PropTypes.EarPieces,
					Index = style[PedProps.EarPieces].Index,
					Texture = style[PedProps.EarPieces].TextureIndex
				},
				Unknown3 = new Prop
				{
					Type = PropTypes.Unknown3,
					Index = style[PedProps.Unknown3].Index,
					Texture = style[PedProps.Unknown3].TextureIndex
				},
				Unknown4 = new Prop
				{
					Type = PropTypes.Unknown4,
					Index = style[PedProps.Unknown4].Index,
					Texture = style[PedProps.Unknown4].TextureIndex
				},
				Unknown5 = new Prop
				{
					Type = PropTypes.Unknown5,
					Index = style[PedProps.Unknown5].Index,
					Texture = style[PedProps.Unknown5].TextureIndex
				},
				Watch = new Prop
				{
					Type = PropTypes.Watches,
					Index = style[PedProps.Watches].Index,
					Texture = style[PedProps.Watches].TextureIndex
				},
				Wristband = new Prop
				{
					Type = PropTypes.Wristbands,
					Index = style[PedProps.Wristbands].Index,
					Texture = style[PedProps.Wristbands].TextureIndex
				},
				Unknown8 = new Prop
				{
					Type = PropTypes.Unknown8,
					Index = style[PedProps.Unknown8].Index,
					Texture = style[PedProps.Unknown8].TextureIndex
				},
				Unknown9 = new Prop
				{
					Type = PropTypes.Unknown9,
					Index = style[PedProps.Unknown9].Index,
					Texture = style[PedProps.Unknown9].TextureIndex
				}
			};
		}
	}
}
