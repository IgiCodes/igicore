using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using IgiCore.SDK.Core.Diagnostics;
using IgiCore.SDK.Core.Helpers;
using IgiCore.SDK.Core.Models;
using IgiCore.SDK.Core.Rpc;
using IgiCore.SDK.Server.Controllers;
using IgiCore.SDK.Server.Events;
using IgiCore.SDK.Server.Rpc;
using Roleplay.Core.Models.Appearance;
using Roleplay.Core.Models.Player;
using Roleplay.Server.Storage;
using RpcEvents = Roleplay.Core.Rpc.RpcEvents;

namespace Roleplay.Server.Controllers
{
	public class CharacterController : ConfigurableController<Configuration>
	{
		public CharacterController(ILogger logger, IEventManager events, IRpcHandler rpc, Configuration configuration) : base(logger, events, rpc, configuration)
		{
			this.Rpc.Event("characters:list").On(Characters);
			this.Rpc.Event(RpcEvents.CharacterCreate).On<Character>(Create);
			this.Rpc.Event(RpcEvents.CharacterDelete).On<Guid>(Delete);
			this.Rpc.Event(RpcEvents.CharacterLoad).On<Guid>(Load);
		}

		public void Characters(IRpcEvent e)
		{
			this.Logger.Info($"Characters: {e.User.Name}");

			using (var context = new CharacterContext())
			{
				var characters = context.Characters.Where(c => c.User.Id == e.User.Id && c.Deleted == null).OrderBy(c => c.Created).ToList();

				e.Reply(characters);
			}
		}

		public async void Create(IRpcEvent e, Character newChar)
		{

			newChar.Id = GuidGenerator.GenerateTimeBasedGuid();
			newChar.UserId = e.User.Id;
			newChar.Model = newChar.Gender == 0 ? "mp_m_freemode_01" : "mp_f_freemode_01";
			newChar.WalkingStyle = "walk like a fucking human being";
			newChar.Alive = true;
			newChar.Health = 10000;
			newChar.Armor = 0;
			newChar.Ssn = 123456789;
			newChar.Position = new Position {X = -1038.121f, Y = -2738.279f, Z = 20.16929f}; // Airport terminal
			//character.Position = new Position { X = 153.7846f, Y = -1032.899f, Z = 29.33798f };	// Legion Square Fleeca
			//character.Position = new Position { X = 892.55f, Y = -182.25f, Z = 73.72f };	// Downtown Cab Co.
			newChar.LastPlayed = null;
			newChar.Created = DateTime.UtcNow;
			newChar.Style = new Style {Id = GuidGenerator.GenerateTimeBasedGuid()};
			//character.Inventory = new Inventory { Id = GuidGenerator.GenerateTimeBasedGuid() };

			try
			{
				using (var context = new CharacterContext())
				{
					context.Characters.Add(newChar);
					await context.SaveChangesAsync();
				}
			}
			catch (DbEntityValidationException ex)
			{
				this.Logger.Debug($"Validation error: {ex.EntityValidationErrors.First().ValidationErrors.First().PropertyName} {ex.EntityValidationErrors.First().ValidationErrors.First().ErrorMessage}");
			}

			e.Reply(newChar);
		}

		public async void Delete(IRpcEvent e, Guid charId)
		{
			using (var context = new CharacterContext())
			{
				context.Configuration.LazyLoadingEnabled = false;
				Character charToDel = await context.Characters.FirstOrDefaultAsync(c => c.Id == charId);
				if (charToDel == null) return;
				charToDel.Deleted = DateTime.UtcNow;
				await context.SaveChangesAsync();
			}
		}

		public async void Load(IRpcEvent e, Guid charId)
		{
			await this.Events.RaiseAsync("roleplay.characters.loading", charId);

			using (var context = new CharacterContext())
			{
				context.Configuration.LazyLoadingEnabled = false;
				Character character = await context.Characters.FirstOrDefaultAsync(c => c.Id == charId);
				if (character == null) return;
				character.LastPlayed = DateTime.UtcNow;
				await context.SaveChangesAsync();
				await this.Events.RaiseAsync("roleplay.characters.loaded", character);
			}

			e.Reply();
		}
	}
}
