using CitizenFX.Core;
using IgiCore.Core.Extensions;
using IgiCore.Core.Models.Appearance;
using IgiCore.Core.Models.Player;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using static IgiCore.Server.Server;

namespace IgiCore.Server.Models
{
	public class Character : ICharacter
	{
		[Key] public Guid Id { get; set; }
		public string Name { get; set; }
		public bool Alive { get; set; }
		public DateTime LastPlayed { get; set; }
		public float PosX { get; set; }
		public float PosY { get; set; }
		public float PosZ { get; set; }
		public virtual Style Style { get; set; }

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
			this.Position = new Vector3 { X = -1038.121f, Y = -2738.279f, Z = 20.16929f };
			this.Alive = false;
		}

		public static Character GetOrCreate(User user, Guid charId)
		{
			Character character = null;
			DbContextTransaction transaction = Db.Database.BeginTransaction();

			try
			{
				if (user.Characters.Count == 0 || user.Characters.All(c => c.Id != charId))
				{
					Debug.WriteLine($"Character not found, creating new char for userid: {user.Id} with id: {charId}");

					character = new Character
					{
						Style = new Style { Id = GuidGenerator.GenerateTimeBasedGuid() }
					};

					user.Characters.Add(character);

					Db.Users.AddOrUpdate(user);
					Db.SaveChanges();
				}
				else
				{
					character = user.Characters.First(c => c.Id == charId);
					Debug.WriteLine($"Character found for userId: {user.Id}  ID: {charId}");
				}

				transaction.Commit();
			}
			catch (Exception ex)
			{
				transaction.Rollback();

				Debug.Write(ex.Message);
			}

			return character;
		}

		public static void Save(Character newChar)
		{
			Log("Character save called");

			Db.Styles.AddOrUpdate(newChar.Style);
			Db.Characters.AddOrUpdate(newChar);
			Db.SaveChanges();
		}

		public override string ToString()
		{
			return $"Character [{this.Id}]: {this.Name}, {this.Position}";
		}
	}
}
