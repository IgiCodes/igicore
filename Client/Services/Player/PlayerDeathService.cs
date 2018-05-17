using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using IgiCore.Client.Utility;
using static CitizenFX.Core.Native.API;

namespace IgiCore.Client.Services.Player
{
	public class PlayerDeathService : ClientService
	{
		protected readonly List<Entity> TrackedEntities = new List<Entity>();

		public event EventHandler<EventArgs> OnDowned;
		public event EventHandler<EventArgs> OnRevived;

		public bool IsDowned { get; protected set; }
		public int? DownedAt { get; protected set; }

		public Entity LastKiller { get; protected set; }

		public PlayerDeathService()
		{
			SetFadeOutAfterDeath(false);
		}

		public override async Task Tick()
		{
			new Text($"Health: {Game.Player.Character.Health} | Killer: {this.LastKiller?.Handle ?? 0} | Ground: {CitizenFX.Core.World.GetGroundHeight(Game.Player.Character.Position)} | Pos: {Game.Player.Character.Position} | Dir: {Game.Player.Character.Heading}", new PointF(20, 10), 0.25f).Draw();
		    new Text($"Sequence Status: {Game.Player.Character.TaskSequenceProgress} | Performing Task: {IsPedUsingAnyScenario(Game.Player.Character.Handle)} | Walking: {Game.Player.Character.IsWalking}", new PointF(50, 50), 0.4f, Color.FromArgb(255, 255, 255), Font.ChaletLondon, Alignment.Left, false, true).Draw();

            // Disable heath regeneration
            SetPlayerHealthRechargeMultiplier(Game.Player.Handle, 0);

			if (Game.Player.Character.IsAlive && !this.IsDowned) return;

			SetPauseMenuActive(true);

			if (Game.Player.Character.IsDead && !this.IsDowned)
			{
				this.IsDowned = true;
				this.DownedAt = Game.GameTime;
				this.LastKiller = Game.Player.Character.GetKiller();

				this.LastKiller.IsPersistent = true;
				this.TrackedEntities.Add(this.LastKiller);

				foreach (var ped in new PedList().Where(p => p.IsInCombat && p.IsInCombatAgainst(Game.Player.Character) && !p.IsPlayer))
				{
					ped.IsPersistent = true;
					this.TrackedEntities.Add(ped);

					ped.Task.ClearAll(); // Cancel fighting
					ped.IsEnemy = false; // Hide map blip

					ped.PlayAmbientSpeech("GENERIC_FUCK_YOU", SpeechModifier.ForceShouted); // Shout

					if (ped.LastVehicle != null && ped.LastVehicle.Exists() && ped.LastVehicle.IsDriveable && !ped.LastVehicle.IsOnFire && ped.LastVehicle.IsSeatFree(VehicleSeat.Driver)) // && ped.LastVehicle.IsNearEntity(ped, new Vector3(5)))
					{
						ped.LastVehicle.IsPersistent = true;
						this.TrackedEntities.Add(ped.LastVehicle);

						ped.Task.EnterVehicle(ped.LastVehicle, VehicleSeat.Driver);
						ped.Task.CruiseWithVehicle(ped.LastVehicle, 100);
						
						ped.DrivingSpeed = 100;
						ped.MaxDrivingSpeed = 100;

						ped.DrivingStyle = DrivingStyle.IgnoreLights | DrivingStyle.Rushed;
						ped.VehicleDrivingFlags = VehicleDrivingFlags.AllowGoingWrongWay | VehicleDrivingFlags.IgnorePathFinding; // VehicleDrivingFlags.AvoidVehicles | VehicleDrivingFlags.AvoidPeds | VehicleDrivingFlags.AvoidObjects;

						SetDriverAggressiveness(ped.Handle, 100); // Max aggression
						SetDriverAbility(ped.Handle, 100);
					}
					else
					{
						ped.Task.ReactAndFlee(Game.Player.Character); // Run
					}
				}

				//ResetLocalplayerState();
				//ResetPlayerArrestState(Game.Player.Handle);
				//IgnoreNextRestart(true);
				//DisableAutomaticRespawn(true);

				//SetFadeInAfterDeathArrest(true);
				//SetFadeOutAfterDeath(false);
				//NetworkRequestControlOfEntity(Game.Player.ActiveCharacter.Handle);

				//Game.Player.ActiveCharacter.Task.ClearAllImmediately();

				//NetworkResurrectLocalPlayer(Game.PlayerPed.Position.X, Game.PlayerPed.Position.Y, Game.PlayerPed.Position.Z, 0, false, false);
				//ResurrectPed(Game.Player.Handle);
				Game.Player.Character.Resurrect();

				//Game.Player.ActiveCharacter.Task.ClearAllImmediately();

				this.OnDowned?.Invoke(this, EventArgs.Empty);
			}

			foreach (Entity entity in this.TrackedEntities)
			{
				entity.IsPersistent = false;
			}
			this.TrackedEntities.Clear();

			Game.Player.Character.Ragdoll();
			Game.Player.Character.IsInvincible = true;
			Game.Player.CanControlRagdoll = true;
			Game.Player.Character.Health = 1;
			Game.Player.WantedLevel = 0;
		}

		public void Revive()
		{
			Game.Player.Character.Resurrect();

			Game.Player.Character.Health = 1; // TODO?

			//Game.Player.ActiveCharacter.ResetVisibleDamage();
			//Game.Player.ActiveCharacter.ClearBloodDamage();

			this.IsDowned = false;
			this.DownedAt = null;

			this.OnRevived?.Invoke(this, EventArgs.Empty);
		}
	}
}
