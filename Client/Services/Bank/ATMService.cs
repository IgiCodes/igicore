using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using IgiCore.Client.Utility;
using IgiCore.Core.Extensions;

namespace IgiCore.Client.Services.Bank
{
    public class AtmService : ClientService
    {
	    public readonly List<int> Models = new List<int>
	    {
		    // prop_fleeca_atm
		    506770882
		};

		public override async Task Tick()
		{
			if (Game.Player.Character.IsInVehicle()) return;

            foreach (var atm in new ObjectList().Where(o => o.Model.IsValid && this.Models.Contains(o.Model.Hash) && Vector3.Distance(Game.Player.Character.Position, o.Position) < 2.0f)) // All nearby ATMs
            {
				if (!Vector3.Distance(Game.Player.Character.Position, atm.Position).IsBetween(1.0f, 1.5f)) continue;
				if (Game.Player.Character.ForwardVector.DistanceToSquared(atm.ForwardVector) > 0.5f) continue;
				
				Vector3 toPlayer = atm.Position - Game.Player.Character.Position;
				toPlayer.Normalize();
				float toPlayerDot = Vector3.Dot(Game.Player.Character.ForwardVector, toPlayer);
	            if (!toPlayerDot.IsBetween(0f, 0.8f)) continue;

				new Text($"ATM {atm.Handle} is nearby", new PointF(50, Screen.Height - 50), 0.4f, Color.FromArgb(255, 255, 255), Font.ChaletLondon, Alignment.Left, false, true).Draw();
			}
		}
    }
}
