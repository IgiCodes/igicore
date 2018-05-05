using System;
using System.Drawing;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;

namespace IgiCore.Client.Interface.Hud.Elements
{
	public class Speedometer : Element
	{
		public Speedometer(HudManager manager) : base(manager) { }

		public override async Task Render()
		{
			if (!Game.Player.Character.IsInVehicle()) return;

			new Text($"{Math.Round(Game.Player.Character.CurrentVehicle.Speed * 2.236936)} MPH", new PointF(120, Screen.Height - 160), 0.4f, Color.FromArgb(220, 220, 220), Font.ChaletComprimeCologne, Alignment.Center, false, true).Draw();
		}
	}
}
