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

			this.Events.Raise("characters:loaded", this.Characters);

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

		protected void OnNuiCharacterLoad(dynamic id, CallbackDelegate callback)
		{
			this.Rpc
				.Event(RpcEvents.CharacterLoad)
				.Trigger(id);

			callback("ok");
		}

		protected async void OnNuiCharacterDelete(dynamic id, CallbackDelegate callback)
		{
			Guid deletedId = await this.Rpc.Event(RpcEvents.CharacterDelete)
				.Request<Guid>(new Guid(id.ToString()));

			Character character = this.Characters.First(c => c.Id == deletedId);
			this.Characters.Remove(character);
			this.Events.Raise("roleplay.characters.deleted", character);
			this.Nui.Send("characters", this.Characters);
			Show();

			callback("ok");
		}

		protected void OnNuiDisconnect(dynamic _, CallbackDelegate callback)
		{
			this.Rpc
				.Event(RpcEvents.ClientDisconnect)
				.Trigger();

			callback("ok");
		}

		protected void OnCharacterCreated(Character newCharacter)
		{
			this.Events.Raise("roleplay.character.created", newCharacter);
			this.Nui.Send("characters", this.Characters);
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
			//Game.Player.Freeze();

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

			// Freeze
			//Game.Player.Unfreeze();

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
