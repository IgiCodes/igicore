using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using IgiCore.Client.Controllers;
using IgiCore.Client.Interface.Hud;
using JetBrains.Annotations;

namespace IgiCore.Client.Interface.Screens
{
	[PublicAPI]
	public class InventoryScreen : Screen
	{
		public InventoryScreen()
		{
			Nui.RegisterCallback("inventory-hide", async (a, b) => await this.Hide());

			TickHandler.Attach<InventoryScreen>(this.Render);
		}

		public override async Task Show()
		{
			if (this.Visible) return;

			// HUD
			Client.Instance.Managers.First<HudManager>().Visible = false;
			API.SetNuiFocus(true, true);

			// Show
			Nui.Send("element:inventory:show");
			this.Visible = true;
		}

		public override async Task Hide()
		{
			if (!this.Visible) return;
			Client.Log("Inventory Hide called!");

			// HUD
			Client.Instance.Managers.First<HudManager>().Visible = true;
			API.SetNuiFocus(false, false);

			// Hide
			Client.Log("Sending Inventory NUI Hide");
			Nui.Send("element:inventory:hide");
			this.Visible = false;
		}

		public override async Task Render()
		{
			if (Input.Input.IsControlJustPressed(Control.PhoneUp) && !this.Visible) await this.Show();
		}
	}
}
