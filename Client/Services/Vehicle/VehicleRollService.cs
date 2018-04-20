using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace IgiCore.Client.Services.Vehicle
{
	public class VehicleRollService : ClientService
	{
		public override async Task Tick()
		{
			if (!Game.Player.Character.IsInVehicle()) return;
			if (Game.Player.Character.IsInFlyingVehicle) return;
			if (Game.Player.Character.IsOnBike) return;

			var roll = API.GetEntityRoll(Game.Player.Character.Handle);
			if (roll > 90.9 || roll < -90.9)
			{
				Game.Player.Character.CurrentVehicle.IsEngineRunning = false;
				API.SetVehicleOutOfControl(Game.Player.Character.CurrentVehicle.Handle, false, false);
			}
		}
	}
}
