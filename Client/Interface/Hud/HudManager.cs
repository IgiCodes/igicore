using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using IgiCore.Client.Handlers;
using IgiCore.Client.Interface.Hud.Elements;
using IgiCore.Client.Managers;
using JetBrains.Annotations;

namespace IgiCore.Client.Interface.Hud
{
	[PublicAPI]
	public class HudManager : Manager
	{
		protected string ServerNameValue = string.Empty;
		protected bool ChatVisibleValue = true;

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

		public bool ChatVisible
		{
			get => this.ChatVisibleValue;
			set
			{
				this.ChatVisibleValue = value;

				API.SetTextChatEnabled(value);
			}
		}

		public HudManager()
		{
			TickHandler.Attach<HudManager>(Render);

			Client.Instance.OnClientReady += async (s, a) =>
			{
				this.ServerName = a.Information.ServerName; // Set pause screen menu server name

				// Init
				API.SetPauseMenuActive(true);
				API.SetNoLoadingScreen(true);

				this.Visible = false;
				this.MiniMapVisible = false;
				this.ChatVisible = false;

				// Fade out screen
				await UI.FadeScreenOut(500);
				UI.ShutdownLoadingScreen();
			};

			this.Elements.Add(new Location(this));
			this.Elements.Add(new Speedometer(this));

			//MiniMapAnchor();
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

			foreach (var element in this.Elements) await element.Render();

			//var ui = MiniMapAnchor();
			//var thickness = 4;
			//drawRct(ui.X, ui.Y, ui.Width, thickness * ui.ScaleY, 0, 255, 0, 100);
			//drawRct(ui.X, ui.Y + ui.Height, ui.Width, -thickness * ui.ScaleY, 0, 255, 0, 100);
			//drawRct(ui.X, ui.Y, thickness * ui.ScaleX, ui.Height, 0, 255, 0, 100);
			//drawRct(ui.X + ui.Width, ui.Y, -thickness * ui.ScaleX, ui.Height, 0, 255, 0, 100);
		}

		protected void drawRct(float x, float y, float width, float height, int r, int g, int b, int a)
		{
			API.DrawRect(x + width / 2, y + height / 2, width, height, r, g, b, a);
		}

		public MiniMapAnchor MiniMapAnchor()
		{
			int width = Screen.Resolution.Width;
			int height = Screen.Resolution.Height;
			float xScale = 1.0f / width;
			float yScale = 1.0f / height;

			float safezoneX = 1.0f / 20.0f;
			float safezoneY = 1.0f / 20.0f;
			float safezone = API.GetSafeZoneSize();

			MiniMapAnchor minimap = new MiniMapAnchor
			{
				Width = xScale * (width / (4 * Screen.AspectRatio)),
				Height = yScale * (height / 5.674f),
				X = xScale * (width * (safezoneX * ((Math.Abs(safezone - 1)) * 10))),
				ScaleX = xScale,
				ScaleY = yScale
			};

			minimap.Y = (1 - yScale * (height * (safezoneY * ((Math.Abs(safezone - 1)) * 10)))) - minimap.Height;

			//Client.Log(minimap.ToString());

			return minimap;
		}

		public override void Dispose()
		{
			TickHandler.Dettach<HudManager>();
		}
	}

	public class MiniMapAnchor
	{
		public float Width { get; set; }
		public float Height { get; set; }
		public float X { get; set; }
		public float Y { get; set; }
		public float ScaleX { get; set; }
		public float ScaleY { get; set; }

		public override string ToString() => $"MiniMapAnchor: Size {this.Width}x{this.Height}, Pos {this.X}x{this.Y}, Scale {this.ScaleX}x{this.ScaleY}";
	}
}
