using System;
using System.Drawing;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using IgiCore.Client.Events;
using IgiCore.Client.Extensions;
using IgiCore.Client.Handlers;
using IgiCore.Client.Interface.Hud;
using IgiCore.Client.Models;
using JetBrains.Annotations;
using Newtonsoft.Json;

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
			Client.Instance.OnClientReady += (sender, args) => NUI.Send("ready", args.Information);
			Client.Instance.OnUserLoaded += (sender, args) => NUI.Send("user", args.User);
			Client.Instance.OnCharactersList += OnCharactersList;

			// NUI events
			NUI.RegisterCallback("character-create", OnNuiCharacterCreate);
			NUI.RegisterCallback("character-load", OnNuiCharacterLoad);
			NUI.RegisterCallback("character-delete", OnNuiCharacterDelete);

			TickHandler.Attach<CharacterSelectScreen>(Render);
		}

		private async void OnCharactersList(object sender, CharactersEventArgs args)
		{
			NUI.Send("characters", args.Characters);

			await Show();

			await UI.FadeScreenIn(500);
		}

		protected void OnNuiCharacterCreate(dynamic character, CallbackDelegate callback)
		{
			BaseScript.TriggerServerEvent("igi:character:create", JsonConvert.SerializeObject(new Character
			{
				Forename = character.forename,
				Middlename = character.middlename,
				Surname = character.surname,
				Gender = short.Parse(character.gender),
				DateOfBirth = DateTime.Parse(character.dob)
			}));

			callback("ok");
		}

		protected void OnNuiCharacterLoad(dynamic id, CallbackDelegate callback)
		{
			BaseScript.TriggerServerEvent("igi:character:load", id);

			callback("ok");
		}

		protected void OnNuiCharacterDelete(dynamic id, CallbackDelegate callback)
		{
			BaseScript.TriggerServerEvent("igi:character:delete", id);

			callback("ok");
		}

		public override async Task Show()
		{
			// HUD
			Client.Instance.Managers.First<HudManager>().Visible = false;
			Client.Instance.Managers.First<HudManager>().ChatVisible = false;
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
		}

		public override async Task Hide()
		{
			// HUD
			Client.Instance.Managers.First<HudManager>().Visible = true;
			Client.Instance.Managers.First<HudManager>().ChatVisible = true;
			API.SetNuiFocus(false, false);

			// Freeze
			Game.Player.Unfreeze();

			// Weather
			API.ClearOverrideWeather();
			API.ClearWeatherTypePersist();

			// Camera
			World.DestroyAllCameras();
			World.RenderingCamera = null; // Required to reset the camera
		}

		public override async Task Render()
		{
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
