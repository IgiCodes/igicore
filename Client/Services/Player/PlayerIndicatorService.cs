using System.Drawing;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace IgiCore.Client.Services.Player
{
	public class PlayerIndicatorService : ClientService
	{
		public override async Task Tick()
		{
			// TODO: Loop all players but local

			CitizenFX.Core.World.DrawMarker(MarkerType.HorizontalCircleSkinny, new Vector3(Game.Player.Character.Position.X, Game.Player.Character.Position.Y, Game.Player.Character.Position.Z - 0.95f), Vector3.Zero, Vector3.Zero, Vector3.One, Color.FromArgb(50, 239, 239, 239));
		}
	}
}
