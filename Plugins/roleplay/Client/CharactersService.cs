using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Roleplay.Core.Models.Player;
using IgiCore.SDK.Client.Events;
using IgiCore.SDK.Client.Extensions;
using IgiCore.SDK.Client.Rpc;
using IgiCore.SDK.Client.Services;
using IgiCore.SDK.Core.Diagnostics;
using CitizenFX.Core.UI;
using IgiCore.SDK.Client.Interface;
using IgiCore.SDK.Core.Models.Player;
using IgiCore.SDK.Core.Rpc;
using JetBrains.Annotations;
using RpcEvents = Roleplay.Core.Rpc.RpcEvents;
using SdkRpcEvents = IgiCore.SDK.Core.Rpc.RpcEvents;

namespace Roleplay.Client
{
	[PublicAPI]
	public class CharactersService : Service
	{
		protected bool Visible { get; set; }
		protected float CameraHeight => 500;
		protected float CameraRadius => 1500;
		protected float CameraAngle { get; set; } = 0;
		protected Vector3 CameraCenter => Vector3.Zero;
		protected DateTime Time => new DateTime(DateTime.Now.Year, 1, 1, 12, 0, 0); // Noon
		protected Weather Weather => Weather.ExtraSunny;
		protected List<Character> Characters = new List<Character>();
		protected Character ActiveCharacter;

		public CharactersService(ILogger logger, ITickManager ticks, IEventManager events, IRpcHandler rpc, INuiManager nui, User user) : base(logger, ticks, events, rpc, nui, user) { }

		public override async Task Loaded()
		{
			// NUI events
			this.Nui.Attach("rules-agreed", OnNuiRulesAgreed);
			this.Nui.Attach("character-create", OnNuiCharacterCreate);
			this.Nui.Attach("character-load", OnNuiCharacterLoad);
			this.Nui.Attach("character-delete", OnNuiCharacterDelete);
			this.Nui.Attach("disconnect", OnNuiDisconnect);

			await Task.FromResult(0);
		}

		public override async Task Started()
		{
			this.Characters = await this.Rpc.Event("characters:list").Request<List<Character>>();

			this.Logger.Debug($"Got {this.Characters.Count} characters");

			await this.Events.RaiseAsync("characters:loaded", this.Characters);

			this.Ticks.Attach(Render);

			API.SetPauseMenuActive(true); // TODO: When?
			API.SetNoLoadingScreen(true);
			
			// Fade out screen
			await FadeScreenOut(500);

			this.Visible = true;

			API.ShutdownLoadingScreen();

			Show();

			this.Nui.Send("user", this.User);
			this.Nui.Send("characters", this.Characters);
			// Fade in screen
			await FadeScreenIn(500);
		}

		protected void OnNuiRulesAgreed(dynamic _, CallbackDelegate callback)
		{
			this.Rpc
				.Event(RpcEvents.AcceptRules)
				.Trigger(DateTime.UtcNow);

			callback("ok");
		}

		protected async void OnNuiCharacterCreate(dynamic character, CallbackDelegate callback)
		{
			Character newCharacter = await this.Rpc.Event(RpcEvents.CharacterCreate)
				.Request<Character>(new Character
				{
					Forename = character.forename,
					Middlename = character.middlename,
					Surname = character.surname,
					Gender = (short)character.gender,
					DateOfBirth = DateTime.Parse(character.dob)
				});
			
			OnCharacterCreated(newCharacter);

			Show();

			callback("ok");
		}

		protected async void OnNuiCharacterDelete(dynamic id, CallbackDelegate callback)
		{
			Guid guid = new Guid(id.ToString());
			this.Rpc.Event(RpcEvents.CharacterDelete)
				.Trigger(guid);

			Character character = this.Characters.First(c => c.Id == guid);
			this.Characters.Remove(character);
			await this.Events.RaiseAsync("roleplay.characters.deleted", character);
			this.Nui.Send("characters", this.Characters);
			Show();

			callback("ok");
		}

		protected void OnNuiDisconnect(dynamic _, CallbackDelegate callback)
		{
			this.Rpc
				.Event(SdkRpcEvents.ClientDisconnect)
				.Trigger();

			callback("ok");
		}

		protected async void OnCharacterCreated(Character newCharacter)
		{
			this.Characters.Add(newCharacter);
			await this.Events.RaiseAsync("roleplay.character.created", newCharacter);
			this.Nui.Send("characters", this.Characters);
		}

		protected async void OnNuiCharacterLoad(dynamic id, CallbackDelegate callback)
		{
			Guid guid = new Guid(id.ToString());
			await this.Rpc
				.Event(RpcEvents.CharacterLoad)
				.Request(guid);

			Character loadedCharacter = this.Characters.FirstOrDefault(c => c.Id == guid);
			if (loadedCharacter == null) return; // TODO: Handle trying to load character with invalid GUID
			this.ActiveCharacter = loadedCharacter;
			await this.Events.RaiseAsync("roleplay.characters.loaded", loadedCharacter);

			await this.Spawn();
			this.Hide();

			callback("ok");

			await this.Events.RaiseAsync("roleplay.characters.spawned", loadedCharacter);

		}

		public async Task Spawn()
		{
			Game.Player.Freeze();

			await this.Sync();

			// Load map
			API.LoadScene(this.ActiveCharacter.Position.X, this.ActiveCharacter.Position.Y, this.ActiveCharacter.Position.Z);
			API.RequestCollisionAtCoord(this.ActiveCharacter.Position.X, this.ActiveCharacter.Position.Y, this.ActiveCharacter.Position.Z);

			// Set Defaults
			Game.PlayerPed.Style.SetDefaultClothes();
			Game.PlayerPed.ClearBloodDamage();
			Game.PlayerPed.Weapons.Drop();
			Game.Player.WantedLevel = 0;

			Game.Player.Unfreeze();
		}

		public async Task Sync()
		{
			Game.Player.Character.Position = this.ActiveCharacter.Position.ToVector3();

			while (!await Game.Player.ChangeModel(new Model(this.ActiveCharacter.Model))) await BaseScript.Delay(10);

			Game.Player.Character.Style[PedComponents.Face].SetVariation(this.ActiveCharacter.Style.Face.Index, this.ActiveCharacter.Style.Face.Texture);
			Game.Player.Character.Style[PedComponents.Head].SetVariation(this.ActiveCharacter.Style.Head.Index, this.ActiveCharacter.Style.Head.Texture);
			Game.Player.Character.Style[PedComponents.Hair].SetVariation(this.ActiveCharacter.Style.Hair.Index, this.ActiveCharacter.Style.Hair.Texture);
			Game.Player.Character.Style[PedComponents.Torso].SetVariation(this.ActiveCharacter.Style.Torso.Index, this.ActiveCharacter.Style.Torso.Texture);
			Game.Player.Character.Style[PedComponents.Legs].SetVariation(this.ActiveCharacter.Style.Legs.Index, this.ActiveCharacter.Style.Legs.Texture);
			Game.Player.Character.Style[PedComponents.Hands].SetVariation(this.ActiveCharacter.Style.Hands.Index, this.ActiveCharacter.Style.Hands.Texture);
			Game.Player.Character.Style[PedComponents.Shoes].SetVariation(this.ActiveCharacter.Style.Shoes.Index, this.ActiveCharacter.Style.Shoes.Texture);
			Game.Player.Character.Style[PedComponents.Special1].SetVariation(this.ActiveCharacter.Style.Special1.Index, this.ActiveCharacter.Style.Special1.Texture);
			Game.Player.Character.Style[PedComponents.Special2].SetVariation(this.ActiveCharacter.Style.Special2.Index, this.ActiveCharacter.Style.Special2.Texture);
			Game.Player.Character.Style[PedComponents.Special3].SetVariation(this.ActiveCharacter.Style.Special3.Index, this.ActiveCharacter.Style.Special3.Texture);
			Game.Player.Character.Style[PedComponents.Textures].SetVariation(this.ActiveCharacter.Style.Textures.Index, this.ActiveCharacter.Style.Textures.Texture);
			Game.Player.Character.Style[PedComponents.Torso2].SetVariation(this.ActiveCharacter.Style.Torso2.Index, this.ActiveCharacter.Style.Torso2.Texture);

			Game.Player.Character.Style[PedProps.Hats].SetVariation(this.ActiveCharacter.Style.Hat.Index, this.ActiveCharacter.Style.Hat.Texture);
			Game.Player.Character.Style[PedProps.Glasses].SetVariation(this.ActiveCharacter.Style.Glasses.Index, this.ActiveCharacter.Style.Glasses.Texture);
			Game.Player.Character.Style[PedProps.EarPieces].SetVariation(this.ActiveCharacter.Style.EarPiece.Index, this.ActiveCharacter.Style.EarPiece.Texture);
			Game.Player.Character.Style[PedProps.Unknown3].SetVariation(this.ActiveCharacter.Style.Unknown3.Index, this.ActiveCharacter.Style.Unknown3.Texture);
			Game.Player.Character.Style[PedProps.Unknown4].SetVariation(this.ActiveCharacter.Style.Unknown4.Index, this.ActiveCharacter.Style.Unknown4.Texture);
			Game.Player.Character.Style[PedProps.Unknown5].SetVariation(this.ActiveCharacter.Style.Unknown5.Index, this.ActiveCharacter.Style.Unknown5.Texture);
			Game.Player.Character.Style[PedProps.Watches].SetVariation(this.ActiveCharacter.Style.Watch.Index, this.ActiveCharacter.Style.Watch.Texture);
			Game.Player.Character.Style[PedProps.Wristbands].SetVariation(this.ActiveCharacter.Style.Wristband.Index, this.ActiveCharacter.Style.Wristband.Texture);
			Game.Player.Character.Style[PedProps.Unknown8].SetVariation(this.ActiveCharacter.Style.Unknown8.Index, this.ActiveCharacter.Style.Unknown8.Texture);
			Game.Player.Character.Style[PedProps.Unknown9].SetVariation(this.ActiveCharacter.Style.Unknown9.Index, this.ActiveCharacter.Style.Unknown9.Texture);
		}

		public void Show()
		{
			this.Logger.Debug("Show called!");
			// HUD
			//Client.Instance.Managers.First<HudManager>().Visible = false;
			API.SetNuiFocus(true, true);

			// Position
			API.LoadScene(this.CameraCenter.X, this.CameraCenter.Y, this.CameraCenter.Z);
			API.RequestCollisionAtCoord(this.CameraCenter.X, this.CameraCenter.Y, this.CameraCenter.Z);

			Game.Player.Character.Position = Vector3.Zero;

			// Freeze
			Game.Player.Freeze();

			// Time
			World.CurrentDayTime = this.Time.TimeOfDay;
			API.NetworkOverrideClockTime(this.Time.Hour, this.Time.Minute, this.Time.Second);

			// Weather
			API.ClearOverrideWeather();
			API.ClearWeatherTypePersist();
			API.SetWeatherTypePersist(Enum.GetName(typeof(Weather), this.Weather)?.ToUpper());
			API.SetWeatherTypeNow(Enum.GetName(typeof(Weather), this.Weather)?.ToUpper());
			API.SetWeatherTypeNowPersist(Enum.GetName(typeof(Weather), this.Weather)?.ToUpper());

			// Camera
			var camera = World.CreateCamera(Vector3.Zero, Vector3.Zero, 50);
			var location = this.CameraCenter.RotateAround(this.CameraRadius, this.CameraAngle);
			camera.Position = new Vector3(location.X, location.Y, this.CameraHeight);
			camera.PointAt(this.CameraCenter);
			World.RenderingCamera = camera;

			this.Visible = true;
		}

		public void Hide()
		{
			// HUD
			//Client.Instance.Managers.First<HudManager>().Visible = true;
			API.SetNuiFocus(false, false);

			// Weather
			API.ClearOverrideWeather();
			API.ClearWeatherTypePersist();

			// Camera
			World.DestroyAllCameras();
			World.RenderingCamera = null; // Required to reset the camera

			this.Visible = false;
		}

		public async Task Render()
		{
			if (!this.Visible) return;

			// Time
			World.CurrentDayTime = this.Time.TimeOfDay;
			API.NetworkOverrideClockTime(this.Time.Hour, this.Time.Minute, this.Time.Second);

			// Camera
			if (this.CameraAngle >= 360) this.CameraAngle = 0;
			this.CameraAngle += 0.01f;

			var location = this.CameraCenter.RotateAround(this.CameraRadius, this.CameraAngle);
			World.RenderingCamera.Position = new Vector3(location.X, location.Y, this.CameraHeight);

			await Task.FromResult(0);
		}

		public async Task FadeScreenOut(int msec = 0)
		{
			this.Logger.Debug("FadeScreenOut called!");
			Screen.Fading.FadeOut(msec);
			while (Screen.Fading.IsFadingOut) await BaseScript.Delay(10);
		}

		public async Task FadeScreenIn(int msec = 0)
		{
			this.Logger.Debug("FadeScreenIn called!");
			Screen.Fading.FadeIn(msec);
			while (Screen.Fading.IsFadingIn) await BaseScript.Delay(10);
		}
	}
}
