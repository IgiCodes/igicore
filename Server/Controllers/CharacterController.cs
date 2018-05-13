using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using IgiCore.Core.Extensions;
using IgiCore.Core.Models.Appearance;
using IgiCore.Core.Models.Player;
using IgiCore.Core.Rpc;
using IgiCore.Server.Extentions;
using IgiCore.Server.Models.Player;
using IgiCore.Server.Rpc;
using IgiCore.Server.Services;

namespace IgiCore.Server.Controllers
{
    public static class CharacterController
	{
		public static async void List([FromSource] Player player)
		{
			User user = await User.GetOrCreate(player);

			if (user.Characters == null) user.Characters = new List<Character>();

			player
				.Event(RpcEvents.GetCharacters)
				.Attach(user.Characters.NotDeleted().OrderBy(c => c.Created))
				.Trigger();
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

        public static async void Create([FromSource] Player player, string json)
		{
			var response = RpcResponse<Character>.Parse(json);

			User user = await User.GetOrCreate(player);

			if (user.Characters == null) user.Characters = new List<Character>();

			Character character = response.Result;
			character.Id = GuidGenerator.GenerateTimeBasedGuid();
			character.Alive = true;
			character.Health = 10000;
			character.Armor = 0;
			character.Ssn = "123-45-6789";
			//character.Position = new Vector3 { X = -1038.121f, Y = -2738.279f, Z = 20.16929f }; // Airport terminal
			//character.Position = new Vector3 { X = 153.7846f, Y = -1032.899f, Z = 29.33798f }; // Legion Square Fleeca
			character.Position = new Vector3 { X = 892.55f, Y = -182.25f, Z = 73.72f }; // Downtown Cab Co.
			character.LastPlayed = null;
			character.Created = DateTime.UtcNow;
			character.Style = new Style { Id = GuidGenerator.GenerateTimeBasedGuid() };
			//character.Inventory = new Inventory { Id = GuidGenerator.GenerateTimeBasedGuid() };

			foreach (ServerService service in Server.Instance.Services) character = await service.OnCharacterCreate(character);

			user.Characters.Add(character);

			Server.Db.Users.AddOrUpdate(user);
			await Server.Db.SaveChangesAsync();
			
			player
				.Event(RpcEvents.GetCharacters)
				.Attach(user.Characters.NotDeleted().OrderBy(c => c.Created))
				.Trigger();
		}

		public static async void Delete([FromSource] Player player, string json)
		{
			var response = RpcResponse<Guid>.Parse(json);

			User user = await User.GetOrCreate(player);

			if (user.Characters == null) user.Characters = new List<Character>();

			var character = user.Characters.FirstOrDefault(c => c.Id == response.Result);

			if (character == null) return;

			character.Deleted = DateTime.UtcNow;

			Server.Db.Characters.AddOrUpdate(character);
			await Server.Db.SaveChangesAsync();

			player
				.Event(RpcEvents.GetCharacters)
				.Attach(user.Characters.NotDeleted().OrderBy(c => c.Created))
				.Trigger();
		}

		public static async void Load([FromSource] Player player, string json)
		{
			var response = RpcResponse<Guid>.Parse(json);

			User user = await User.GetOrCreate(player);

			if (user.Characters == null) user.Characters = new List<Character>();

			var character = user.Characters.NotDeleted().FirstOrDefault(c => c.Id == response.Result);

			if (character == null) return;

			character.LastPlayed = DateTime.UtcNow;

			Server.Db.Characters.AddOrUpdate(character);
			await Server.Db.SaveChangesAsync();

			player
				.Event(RpcEvents.CharacterLoad)
				.Attach(character)
				.Trigger();
		}
		
		public static async void Save([FromSource] Player player, string json)
		{
			var response = RpcResponse<Character>.Parse(json);

			var newChar = response.Result;
			Server.Db.Styles.AddOrUpdate(newChar.Style);
			Server.Db.Characters.AddOrUpdate(newChar);

			await Server.Db.SaveChangesAsync();
		}
	}
}
