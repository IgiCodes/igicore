using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using CitizenFX.Core;
using IgiCore.Core;
using IgiCore.Core.Extensions;
using IgiCore.Core.Models.Appearance;
using IgiCore.Core.Rpc;
using IgiCore.Server.Extentions;
using IgiCore.Server.Models.Player;
using IgiCore.Server.Rpc;
using IgiCore.Server.Services;

namespace IgiCore.Server.Handlers
{
    public static class CharacterActions
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
			//character.Position = new Vector3 { X = -1038.121f, Y = -2738.279f, Z = 20.16929f };
			character.Position = new Vector3 { X = 153.7846f, Y = -1032.899f, Z = 29.33798f };
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
