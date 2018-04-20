using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;

namespace IgiCore.Client.Services.World
{
	public class AiPoliceService : ClientService
	{
		protected bool enabled;

		public event EventHandler<EventArgs> OnEnabled;

		public bool Enabled
		{
			get => this.enabled; 
			set
			{
				this.enabled = value;
				Game.ShowsPoliceBlipsOnRadar = this.enabled;

				this.OnEnabled?.Invoke(this, EventArgs.Empty);
			}
		}

		public override async Task Tick()
		{
			// DT_FireDepartment = 3,
			// DT_AmbulanceDepartment = 5,
			// DT_Gangs = 11,
			// DT_ArmyVehicle = 14,
			// DT_BikerBackup = 15
		
			API.EnableDispatchService(1, this.Enabled); // DT_PoliceAutomobile
			API.EnableDispatchService(2, this.Enabled); // DT_PoliceHelicopter
			API.EnableDispatchService(4, this.Enabled); // DT_SwatAutomobile
			API.EnableDispatchService(6, this.Enabled); // DT_PoliceRiders
			API.EnableDispatchService(7, this.Enabled); // DT_PoliceVehicleRequest
			API.EnableDispatchService(8, this.Enabled); // DT_PoliceRoadBlock
			API.EnableDispatchService(9, this.Enabled); // DT_PoliceAutomobileWaitPulledOver
			API.EnableDispatchService(10, this.Enabled); // DT_PoliceAutomobileWaitCruising
			API.EnableDispatchService(12, this.Enabled); // DT_SwatHelicopter
			API.EnableDispatchService(13, this.Enabled); // DT_PoliceBoat

			if (this.Enabled) return;

			Game.Player.WantedLevel = 0;

			Screen.Hud.HideComponentThisFrame(HudComponent.WantedStars);
		}
	}
}
