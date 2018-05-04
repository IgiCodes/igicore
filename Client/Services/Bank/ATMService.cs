using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using IgiCore.Client.Utility;
using IgiCore.Core.Extensions;

namespace IgiCore.Client.Services.Bank
{
	public class AtmService : ClientService
	{
		protected readonly ObjectList Objects = new ObjectList();

		public readonly List<int> Models = new List<int>
		{
			506770882, // prop_fleeca_atm
		    -870868698, // prop_atm_01
		    -1126237515, // prop_atm_02
		    -1364697528, // prop_atm_03
		};

		public override async Task Tick()
		{
			if (Game.Player.Character.IsInVehicle()) return;

			foreach (var atm in this.Objects
				.Where(o => this.Models.Contains(o.Model.Hash)) //	Correct model
				.Where(atm => atm.Position.DistanceToSquared(Game.Player.Character.Position) < 2.0F) // Nearby
				//.Where(atm => Game.Player.Character.ForwardVector.DistanceToSquared(atm.ForwardVector) < 0.5f) // Facing
				.Where(atm => Vector3.Dot(Game.Player.Character.ForwardVector, Vector3.Normalize(atm.Position - Game.Player.Character.Position)).IsBetween(0f, 0.8f)) // In front of
			)
			{
				new Text("Press M to use ATM", new PointF(50, Screen.Height - 50), 0.4f, Color.FromArgb(255, 255, 255), Font.ChaletLondon, Alignment.Left, false, true).Draw();

				if (!Input.Input.IsControlJustPressed(Control.InteractionMenu)) continue;

				Game.Player.Character.Task.GoTo(atm, Vector3.Zero, 2000); // Need to provide an offset or the player tried to walk inside the model
				await BaseScript.Delay(2000);

				Game.Player.Character.Task.TurnTo(atm, 1500);
				await BaseScript.Delay(1500);

				API.SetScenarioTypeEnabled("PROP_HUMAN_ATM", true);
				API.ResetScenarioTypesEnabled();
				API.TaskStartScenarioInPlace(Game.PlayerPed.Handle, "PROP_HUMAN_ATM", 0, true);

				//Game.Player.Character.Task.ClearAllImmediately(); // Cancel animation
			}

			// TODO: Better?
			//Game.Player.Character.Task.PlayAnimation("amb@prop_human_atm@male@enter", "enter");
			//Game.Player.Character.Task.PlayAnimation("amb@prop_human_atm@male@base", "base");
			//Game.Player.Character.Task.PlayAnimation("amb@prop_human_atm@male@idle_a", "idle_a");
			//Game.Player.Character.Task.PlayAnimation("amb@prop_human_atm@male@idle_a", "idle_b");
			//Game.Player.Character.Task.PlayAnimation("amb@prop_human_atm@male@exit", "exit");
		}
	}
}
