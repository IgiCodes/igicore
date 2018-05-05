using System.Drawing;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;

namespace IgiCore.Client.Interface.Hud.Elements
{
	public class Location : Element
	{
		public Location(HudManager manager) : base(manager) { }

		public override async Task Render()
		{
			if (!this.Manager.MiniMapVisible) return;

			var street = World.GetStreetName(Game.Player.Character.Position);
			var crossing = GetCrossingName(Game.Player.Character.Position);
			if (!string.IsNullOrWhiteSpace(crossing)) street += $" & {crossing}";

			new Text(street, new PointF(230, Screen.Height - 45), 0.25f, Color.FromArgb(255, 255, 255), Font.ChaletLondon, Alignment.Left, false, true).Draw();
			new Text(World.GetZoneLocalizedName(Game.Player.Character.Position), new PointF(230, Screen.Height - 30), 0.25f, Color.FromArgb(220, 220, 220), Font.ChaletLondon, Alignment.Left, false, true).Draw();
		}

		private static string GetCrossingName(Vector3 position)
		{
			OutputArgument crossingHash = new OutputArgument();
			Function.Call(Hash.GET_STREET_NAME_AT_COORD, position.X, position.Y, position.Z, new OutputArgument(), crossingHash);

			return API.GetStreetNameFromHashKey(crossingHash.GetResult<uint>());
		}
	}
}
