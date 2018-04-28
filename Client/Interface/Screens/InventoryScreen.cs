using System;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using IgiCore.Client.Events;
using IgiCore.Client.Extensions;
using IgiCore.Client.Handlers;
using IgiCore.Client.Input;
using IgiCore.Client.Interface.Hud;
using IgiCore.Client.Models;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace IgiCore.Client.Interface.Screens
{
	[PublicAPI]
	public class InventoryScreen : Screen
	{
		public bool Visible = false;

		public InventoryScreen()
		{
			NUI.RegisterCallback("inventory-hide", async (a, b) => await this.Hide());

			TickHandler.Attach<InventoryScreen>(this.Render);
		}

		public override async Task Show()
		{
			if (this.Visible) return;

			// HUD
			Client.Instance.Managers.First<HudManager>().Visible = false;
			Client.Instance.Managers.First<HudManager>().ChatVisible = false;
			API.SetNuiFocus(true, true);

			// Show
			NUI.Send("element:inventory:show");
			NUI.Show();
			this.Visible = true;
		}

		public override async Task Hide()
		{
			if (!this.Visible) return;
			Client.Log("Inventory Hide called!");

			// HUD
			Client.Instance.Managers.First<HudManager>().Visible = true;
			Client.Instance.Managers.First<HudManager>().ChatVisible = true;
			API.SetNuiFocus(false, false);

			// Hide
			Client.Log("Sending Inventory NUI Hide");
			NUI.Send("element:inventory:hide");
			NUI.Hide();
			this.Visible = false;
		}

		public override async Task Render()
		{
			if (Input.Input.IsControlJustPressed(Control.PhoneUp) && !this.Visible) await this.Show();
		}
	}
}
