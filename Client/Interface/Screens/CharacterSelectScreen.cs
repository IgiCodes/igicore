using System;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using IgiCore.Client.Controllers;
using IgiCore.Client.Controllers.Player;
using IgiCore.Client.Events;
using IgiCore.Client.Extensions;
using IgiCore.Client.Interface.Hud;
using IgiCore.Client.Models;
using IgiCore.Client.Rpc;
using IgiCore.Core;
using IgiCore.Core.Extensions;
using IgiCore.Core.Rpc;
using JetBrains.Annotations;

namespace IgiCore.Client.Interface.Screens
{
	[PublicAPI]
	public class CharacterSelectScreen : Screen
	{
		public float CameraHeight => 500;
		public float CameraRadius => 1500;
		public float CameraAngle { get; protected set; } = 0;
		public Vector3 CameraCenter => Vector3.Zero;
		public DateTime Time => new DateTime(DateTime.Now.Year, 1, 1, 12, 0, 0); // Noon
		public Weather Weather => Weather.ExtraSunny;

		public CharacterSelectScreen()
		{
			// Events
			Client.Instance.Controllers.First<ClientController>().OnClientReady += (sender, args) => Nui.Send("client", args.Information);
			Client.Instance.Controllers.First<UserController>().OnUserLoaded += (sender, args) => Nui.Send("user", args.User);
			Client.Instance.Controllers.First<CharacterController>().OnCharactersList += (sender, args) => Nui.Send("characters", args.Characters);
			Client.Instance.Controllers.First<CharacterController>().OnCharactersList += OnCharactersList;

			// NUI events
			Nui.RegisterCallback("rules-agreed", OnNuiRulesAgreed);
			Nui.RegisterCallback("character-create", OnNuiCharacterCreate);
			Nui.RegisterCallback("character-load", OnNuiCharacterLoad);
			Nui.RegisterCallback("character-delete", OnNuiCharacterDelete);
			Nui.RegisterCallback("disconnect", OnNuiDisconnect);
		}

		private async void OnCharactersList(object sender, CharactersEventArgs args)
		{
			await Show();
		}

		protected void OnNuiRulesAgreed(dynamic _, CallbackDelegate callback)
		{
			Server
				.Event(RpcEvents.AcceptRules)
				.Attach(DateTime.UtcNow)
				.Trigger();
			
			callback("ok");
		}
		protected async void OnNuiCharacterCreate(dynamic character, CallbackDelegate callback)
		{
			Character newCharacter = await Server.Event(RpcEvents.CharacterCreate)
				.Attach(new Character
				{
					Forename = character.forename,
					Middlename = character.middlename,
					Surname = character.surname,
					Gender = (short)character.gender,
					DateOfBirth = DateTime.Parse(character.dob)
				})
				.Request<Character>();

			Client.Instance.Controllers.First<CharacterController>().Characters.Add(newCharacter);
			Nui.Send("characters", Client.Instance.Controllers.First<CharacterController>().Characters);
			await Show();

			callback("ok");
		}

		protected void OnNuiCharacterLoad(dynamic id, CallbackDelegate callback)
		{
			Server
				.Event(RpcEvents.CharacterLoad)
				.Attach(id)
				.Trigger();

			callback("ok");
		}

		protected async void OnNuiCharacterDelete(dynamic id, CallbackDelegate callback)
		{
			Guid deletedId = await Server.Event(RpcEvents.CharacterDelete)
				.Attach(new Guid(id.ToString()))
				.Request<Guid>();

			Character character = Client.Instance.Controllers.First<CharacterController>().Characters.First(c => c.Id == deletedId);
			Client.Instance.Controllers.First<CharacterController>().Characters.Remove(character);
			Nui.Send("characters", Client.Instance.Controllers.First<CharacterController>().Characters);
			await Show();

			callback("ok");
		}

		protected void OnNuiDisconnect(dynamic _, CallbackDelegate callback)
		{
			Server
				.Event(RpcEvents.ClientDisconnect)
				.Trigger();

			callback("ok");
		}

		public override async Task Show()
		{
			// HUD
			Client.Instance.Managers.First<HudManager>().Visible = false;
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

		public override async Task Hide()
		{
			// HUD
			Client.Instance.Managers.First<HudManager>().Visible = true;
			API.SetNuiFocus(false, false);

			// Freeze
			Game.Player.Unfreeze();

			// Weather
			API.ClearOverrideWeather();
			API.ClearWeatherTypePersist();

			// Camera
			World.DestroyAllCameras();
			World.RenderingCamera = null; // Required to reset the camera

			this.Visible = false;
		}

		public override async Task Render()
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
		}
	}
}
