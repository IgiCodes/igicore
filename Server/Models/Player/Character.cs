using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using IgiCore.Core.Extensions;
using IgiCore.Core.Models.Appearance;
using IgiCore.Core.Models.Inventories.Characters;
using IgiCore.Core.Models.Player;
using Newtonsoft.Json;

namespace IgiCore.Server.Models.Player
{
	public class Character : ICharacter
	{
		[Key] public Guid Id { get; set; }
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
		public virtual Inventory Inventory { get; set; }
		public virtual Style Style { get; set; }
		public DateTime LastPlayed { get; set; }
		public DateTime? Deleted { get; set; }
		public DateTime Created { get; set; }
		public virtual List<Skill> Skills { get; set; }

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

		public Character()
		{
			this.Id = GuidGenerator.GenerateTimeBasedGuid();
			//this.Position = new Vector3 { X = -1038.121f, Y = -2738.279f, Z = 20.16929f };
			this.Position = new Vector3 { X = 153.7846f, Y = -1032.899f, Z = 29.33798f };
			this.Alive = false;
		}

		public static async Task<Character> GetOrCreate(User user, Guid charId)
		{
			Character character = null;
			DbContextTransaction transaction = Server.Db.Database.BeginTransaction();

			try
			{
				if (user.Characters.Count == 0 || user.Characters.All(c => c.Id != charId))
				{
					Debug.WriteLine($"Character not found, creating new char for userid: {user.Id} with id: {charId}");

					character = new Character
					{ Style = new Style { Id = GuidGenerator.GenerateTimeBasedGuid() } };

					user.Characters.Add(character);

					Server.Db.Users.AddOrUpdate(user);
					await Server.Db.SaveChangesAsync();
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

		public static async Task<Character> GetLatestOrCreate(User user)
		{
			Character character = null;
			DbContextTransaction transaction = Server.Db.Database.BeginTransaction();

			try
			{
				if (user.Characters.Count == 0)
				{
					Debug.WriteLine($"Character not found, creating new char for userid: {user.Id} ");

					character = new Character
					{ Style = new Style { Id = GuidGenerator.GenerateTimeBasedGuid() } };

					user.Characters.Add(character);

					Server.Db.Users.AddOrUpdate(user);
					await Server.Db.SaveChangesAsync();
				}
				else
				{
					character = user.Characters.OrderBy(c => c.LastPlayed).Last();
					Debug.WriteLine($"Character found for userId: {user.Id}  ID: {character.Id}");
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

		public static async void Save(Character newChar)
		{
			Server.Log("Character save called");

			Server.Db.Styles.AddOrUpdate(newChar.Style);
			Server.Db.Characters.AddOrUpdate(newChar);
			await Server.Db.SaveChangesAsync();
		}

		public override string ToString() { return $"Character [{this.Id}]: {this.FullName}, {this.Position}"; }
	}
}
