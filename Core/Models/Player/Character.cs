using System;
using System.Collections.Generic;
using CitizenFX.Core;
using IgiCore.Core.Extensions;
using IgiCore.Core.Models.Inventories.Characters;
using Newtonsoft.Json;
using Style = IgiCore.Core.Models.Appearance.Style;

namespace IgiCore.Core.Models.Player
{
	public class Character : Model, ICharacter
	{
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
		public DateTime? LastPlayed { get; set; }

		public virtual List<Skill> Skills { get; set; }
		public virtual Style Style { get; set; }
		public virtual Inventory Inventory { get; set; }

		[JsonIgnore]
		public string FullName => $"{this.Forename} {this.Middlename} {this.Surname}".Replace("  ", " ");

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

		public Character()
		{
			this.Id = GuidGenerator.GenerateTimeBasedGuid();
			this.Alive = false;
			//this.Position = new Vector3 { X = -1038.121f, Y = -2738.279f, Z = 20.16929f };
			this.Position = new Vector3 { X = 153.7846f, Y = -1032.899f, Z = 29.33798f };
			this.Created = DateTime.UtcNow;
		}

		public override string ToString() { return $"Character [{this.Id}]: {this.FullName}, {this.Position}"; }
	}
}
