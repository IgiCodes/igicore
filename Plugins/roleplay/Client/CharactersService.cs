using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using IgiCore.Models.Player;
using IgiCore.SDK.Client.Events;
using IgiCore.SDK.Client.Extensions;
using IgiCore.SDK.Client.Rpc;
using IgiCore.SDK.Client.Services;
using IgiCore.SDK.Core.Diagnostics;
using CitizenFX.Core.UI;

namespace Roleplay.Client
{
	public class CharactersService : Service
	{
		public bool Visible { get; protected set; }
		public float CameraHeight => 500;
		public float CameraRadius => 1500;
		public float CameraAngle { get; protected set; } = 0;
		public Vector3 CameraCenter => Vector3.Zero;
		public DateTime Time => new DateTime(DateTime.Now.Year, 1, 1, 12, 0, 0); // Noon
		public Weather Weather => Weather.ExtraSunny;

		public CharactersService(ILogger logger, ITickManager ticks, IEventManager events, IRpcHandler rpc, User user) : base(logger, ticks, events, rpc, user) { }

		public override async Task Started()
		{
			var characters = await this.Rpc.Event("characters:list").Request<List<Character>>();

			this.Logger.Debug($"Got {characters.Count} characters");

			this.Events.Raise("characters:loaded", characters);

			this.Ticks.Attach(Render);

			API.SetPauseMenuActive(true); // TODO: When?
			API.SetNoLoadingScreen(true);
			
			// Fade out screen
			await FadeScreenOut(500);

			this.Visible = true;

			API.ShutdownLoadingScreen();

			// Fade in screen
			await FadeScreenIn(500);
		}

		public void Show()
		{
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

		public static async Task FadeScreenOut(int msec = 0)
		{
			Screen.Fading.FadeOut(msec);
			while (Screen.Fading.IsFadingOut) await BaseScript.Delay(10);
		}

		public static async Task FadeScreenIn(int msec = 0)
		{
			Screen.Fading.FadeIn(msec);
			while (Screen.Fading.IsFadingIn) await BaseScript.Delay(10);
		}
	}
}
