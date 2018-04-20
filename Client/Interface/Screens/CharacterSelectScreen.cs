using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
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
		public Vector3 Postision => new Vector3(763.5f, 1185.5f, 380); // Top of VINEWOOD sign
		public DateTime Time => new DateTime(DateTime.Now.Year, 1, 1, 12, 0, 0); // Noon
		public Weather Weather => Weather.ExtraSunny;

		public CharacterSelectScreen()
		{
			// Events
			Client.Instance.OnCharactersList += OnCharactersList;

			// NUI events
			RegisterNuiCallback("character-create", OnNuiCharacterCreate);
			RegisterNuiCallback("character-load", OnNuiCharacterLoad);
			RegisterNuiCallback("character-delete", OnNuiCharacterDelete);

			TickHandler.Attach<CharacterSelectScreen>(this.Render);
		}

		private async void OnCharactersList(object sender, CharactersEventArgs args)
		{
			SendNuiMessage("screen:character-creation:characters", args.Characters);

			await Show();
		}

		protected void OnNuiCharacterCreate(dynamic character)
		{
			BaseScript.TriggerServerEvent("igi:character:create", JsonConvert.SerializeObject(new Character
			{
				Forename = character.forename,
				Middlename = character.middlename,
				Surname = character.surname,
				Gender = short.Parse(character.gender),
				DateOfBirth = DateTime.Parse(character.dob)
			}));
		}

		protected void OnNuiCharacterLoad(dynamic id, CallbackDelegate callback)
		{
			BaseScript.TriggerServerEvent("igi:character:load", id);
		}

		protected void OnNuiCharacterDelete(dynamic id)
		{
			BaseScript.TriggerServerEvent("igi:character:delete", id);
		}

		public override async Task Show()
		{
			// HUD
			Client.Instance.Managers.First<HudManager>().Visible = false;
			Client.Instance.Managers.First<HudManager>().ChatVisible = false;
			API.SetNuiFocus(true, true);

			// Position
			API.LoadScene(this.Postision.X, this.Postision.Y, this.Postision.Z);
			API.RequestCollisionAtCoord(this.Postision.X, this.Postision.Y, this.Postision.Z);
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
			var camera = World.CreateCamera(this.Postision, Vector3.Zero, 50);
			camera.PointAt(Vector3.Zero); // Point towards the city
			World.RenderingCamera = camera;

			// Show
			SendNuiMessage("screen:character-creation:show");

			await UI.FadeScreenIn(500);
		}

		public override async Task Hide()
		{
			await UI.FadeScreenOut(500);

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
			
			// Hide
			SendNuiMessage("screen:character-creation:hide");

			await UI.FadeScreenIn(500);
		}

		public override async Task Render()
		{
			// Time
			World.CurrentDayTime = this.Time.TimeOfDay;
			API.NetworkOverrideClockTime(this.Time.Hour, this.Time.Minute, this.Time.Second);
		}
	}
}
