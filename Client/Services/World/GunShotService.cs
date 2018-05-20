using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using IgiCore.Client.Utility;

namespace IgiCore.Client.Services.World
{
	public class GunShotService : ClientService
	{
		protected int Fired = 0;

		public override async Task Tick()
		{
			if (Game.Player.Character.IsShooting)
			{
				this.Fired++;

				Client.Log($"[PLAYER]: Shot at {Game.Player.Character.GetLastWeaponImpactPosition()}");
			}

			foreach (Ped ped in new PedList().Where(p => p.IsShooting))
			{
				Client.Log($"[{ped.Handle}]: Shooting with {ped.Weapons.Current.DisplayName} {ped.Weapons.CurrentWeaponObject.Handle}");
			}

			new Text($"Shots: {this.Fired}", new PointF(50, Screen.Height - 50), 0.4f, Color.FromArgb(255, 255, 255), Font.ChaletLondon, Alignment.Left, false, true).Draw();
		}
	}
}
