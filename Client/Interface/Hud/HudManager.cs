using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using IgiCore.Client.Events;
using IgiCore.Client.Handlers;
using IgiCore.Client.Interface.Hud.Elements;
using IgiCore.Client.Interface.Screens;
using IgiCore.Client.Managers;
using JetBrains.Annotations;
using Screen = CitizenFX.Core.UI.Screen;

namespace IgiCore.Client.Interface.Hud
{
	[PublicAPI]
	public class HudManager : Manager
	{
		protected string ServerNameValue = string.Empty;
		protected bool ChatVisibleValue = true;
		
		public List<Screens.Screen> Screens { get; } = new List<Screens.Screen>();
		public List<Element> Elements { get; } = new List<Element>();

		public string ServerName
		{
			get => this.ServerNameValue;
			set
			{
				this.ServerNameValue = value;

				// Set pause screen title
				Function.Call(Hash.ADD_TEXT_ENTRY, "FE_THDR_GTAO", value);
			}
		}

		public bool Visible
		{
			get => Screen.Hud.IsVisible;
			set => Screen.Hud.IsVisible = value;
		}

		public bool MiniMapVisible
		{
			get => Screen.Hud.IsRadarVisible;
			set => Screen.Hud.IsRadarVisible = value;
		}

		public HudManager()
		{
			TickHandler.Attach<HudManager>(Render);

			Client.Instance.OnClientReady += OnClientReady;
			Client.Instance.OnCharacterLoaded += OnCharacterLoaded;

			//this.Elements.Add(new Location(this));
			//this.Elements.Add(new Speedometer(this));

			this.Screens.Add(new CharacterSelectScreen());
			//this.Screens.Add(new InventoryScreen());

			API.SetTextChatEnabled(false);
		}

		private async void OnClientReady(object s, ServerInformationEventArgs a)
		{
			this.ServerName = a.Information.ServerName; // Set pause screen menu server name
			
			API.SetPauseMenuActive(true); // TODO: When?
			API.SetNoLoadingScreen(true);

			this.Visible = false;
			this.MiniMapVisible = false;

			// Fade out screen
			await UI.FadeScreenOut(500);

			await this.Screens[0].Show();

			UI.ShutdownLoadingScreen();

			// Fade in screen
			await UI.FadeScreenIn(500);
		}

		private async void OnCharacterLoaded(object s, CharacterEventArgs a)
		{
			// Fade out screen
			await UI.FadeScreenOut(500);

			foreach (var screen in this.Screens) await screen.Hide();
			
			// Fade in screen
			await UI.FadeScreenIn(500);
		}

		public async Task Render()
		{
			Screen.Hud.HideComponentThisFrame(HudComponent.WeaponIcon);
			Screen.Hud.HideComponentThisFrame(HudComponent.Cash);
			Screen.Hud.HideComponentThisFrame(HudComponent.MpCash);
			Screen.Hud.HideComponentThisFrame(HudComponent.MpMessage);
			Screen.Hud.HideComponentThisFrame(HudComponent.VehicleName);
			Screen.Hud.HideComponentThisFrame(HudComponent.AreaName);
			Screen.Hud.HideComponentThisFrame(HudComponent.StreetName);
			Screen.Hud.HideComponentThisFrame(HudComponent.HelpText);
			Screen.Hud.HideComponentThisFrame(HudComponent.FloatingHelpText1);
			Screen.Hud.HideComponentThisFrame(HudComponent.FloatingHelpText2);
			Screen.Hud.HideComponentThisFrame(HudComponent.CashChange);
			Screen.Hud.HideComponentThisFrame(HudComponent.SubtitleText);
			Screen.Hud.HideComponentThisFrame(HudComponent.Saving);
			Screen.Hud.HideComponentThisFrame(HudComponent.WeaponWheelStats);

			this.MiniMapVisible = Game.Player.Character.IsInVehicle();

			foreach (var screen in this.Screens) await screen.Render();
			foreach (var element in this.Elements) await element.Render();
		}
		
		public override void Dispose()
		{
			TickHandler.Dettach<HudManager>();
		}
	}
}
