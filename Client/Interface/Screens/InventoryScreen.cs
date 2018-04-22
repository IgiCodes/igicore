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
		public Vector3 Postision => new Vector3(763.5f, 1185.5f, 380); // Top of VINEWOOD sign
		public DateTime Time => new DateTime(DateTime.Now.Year, 1, 1, 12, 0, 0); // Noon
		public Weather Weather => Weather.ExtraSunny;

        public new bool Enabled = false;

		public InventoryScreen()
		{
		    RegisterNuiCallback("inventory-hide", async (a, b) => await this.Hide());
            TickHandler.Attach<InventoryScreen>(this.Render);
		}

		public override async Task Show()
		{
		    if (!this.Enabled || this.Visible) return;

			// HUD
			Client.Instance.Managers.First<HudManager>().Visible = false;
			Client.Instance.Managers.First<HudManager>().ChatVisible = false;
			API.SetNuiFocus(true, true);

			// Show
			SendNuiMessage("element:inventory:show");
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
            SendNuiMessage("element:inventory:hide");
		    this.Visible = false;
		}

	    public override async Task Render()
	    {
	        if (Input.Input.IsControlJustPressed(Control.PhoneUp) && !this.Visible) await this.Show();
	    }
	}
}
