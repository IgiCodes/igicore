using System;
using System.Collections.Generic;
using CitizenFX.Core;
using IgiCore.Client.Events;
using IgiCore.Client.Models;
using IgiCore.Client.Rpc;
using IgiCore.Core.Controllers;
using IgiCore.Core.Rpc;

namespace IgiCore.Client.Controllers.Player
{
	public class CharacterController : Controller
	{
		public event EventHandler<CharactersEventArgs> OnCharactersList;
		public event EventHandler<CharacterEventArgs> OnCharacterLoaded;

		public List<Character> Characters { get; set; }
		public Character ActiveCharacter { get; protected set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="CharacterController"/> class.
		/// </summary>
		public CharacterController()
		{
			Server.Event(RpcEvents.CharacterLoad).On<Character>(this.Load);
			Server.Event(RpcEvents.GetCharacterPosition).On(() => Server.Event(RpcEvents.GetCharacterPosition)
				.Attach(Game.PlayerPed.Position)
				.Attach(Game.PlayerPed.Heading)
				.Trigger()
			);
		}


		/// <summary>
		/// Loads the specified character.
		/// </summary>
		/// <param name="character">The character.</param>
		/// <exception cref="ArgumentNullException">Attempted to load a null character object.</exception>
		public async void Load(Character character)
		{
			Client.Log("igi:character:load");

			// Unload old character
			this.ActiveCharacter?.Dispose();
			// Store the character
			this.ActiveCharacter = character ?? throw new ArgumentNullException(nameof(character));
			// Setup character
			this.ActiveCharacter.Initialize();
			// Render new character
			this.ActiveCharacter.Render();

			this.OnCharacterLoaded?.Invoke(
				Client.Instance,
				new CharacterEventArgs(this.ActiveCharacter)
			);
		}

		/// <summary>
		/// Gets the characters.
		/// </summary>
		public async void GetCharacters()
		{
			this.Characters = await Server
				.Event(RpcEvents.GetCharacters)
				.Request<List<Character>>();

			this.OnCharactersList?.Invoke(this, new CharactersEventArgs(this.Characters));
		}
	}
}
