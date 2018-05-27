using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Banking.Core.Models;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using IgiCore.SDK.Client.Extensions;
using IgiCore.SDK.Client.Input;
using IgiCore.SDK.Client.Rpc;
using IgiCore.SDK.Client.Services;
using IgiCore.SDK.Core.Diagnostics;

namespace Banking.Client
{
	public class AtmService : Service
	{
		private bool inAnimating = false;
		private readonly List<BankAtm> atms;

		public AtmService(ILogger logger, IEventsManager events) : base(logger, events)
		{
			// TODO: Get ATMs from server

			this.atms = new List<BankAtm>();
		}

		public override async Task Tick()
		{
			new Text("PLUGINS!", new PointF(50, Screen.Height - 50), 0.4f, Color.FromArgb(255, 255, 255), Font.ChaletLondon, Alignment.Left, false, true).Draw();

			if (this.inAnimating && Input.IsControlJustPressed(Control.MoveUpOnly))
			{
				Game.Player.Character.Task.ClearAllImmediately(); // Cancel animation
				this.inAnimating = false;
			}

			if (Game.Player.Character.IsInVehicle() || this.inAnimating) return;

			Tuple<BankAtm, Prop> atm = this.atms
				.Select(a => new { atm = a, distance = new Vector3(a.Position.X, a.Position.Y, a.Position.Z).DistanceToSquared(Game.Player.Character.Position) })
				.Where(a => a.distance < 5.0F) // Nearby
				.Select(a => new { a.atm, prop = new Prop(API.GetClosestObjectOfType(a.atm.Position.X, a.atm.Position.Y, a.atm.Position.Z, 1, (uint)a.atm.Hash, false, false, false)), a.distance })
				.Where(p => p.prop.Model.IsValid)
				.Where(a => Vector3.Dot(a.prop.ForwardVector, Vector3.Normalize(a.prop.Position - Game.Player.Character.Position)).IsBetween(0f, 1.0f)) // In front of
				.OrderBy(a => a.distance)
				.Select(a => new Tuple<BankAtm, Prop>(a.atm, a.prop))
				.FirstOrDefault();

			if (atm == null) return;

			new Text("Press M to use ATM", new PointF(50, Screen.Height - 50), 0.4f, Color.FromArgb(255, 255, 255), Font.ChaletLondon, Alignment.Left, false, true).Draw();

			if (!Input.IsControlJustPressed(Control.InteractionMenu)) return;

			var ts = new TaskSequence();
			ts.AddTask.LookAt(atm.Item2);
			ts.AddTask.GoTo(atm.Item2, Vector3.Zero, 2000);
			ts.AddTask.AchieveHeading(atm.Item2.Heading);
			ts.AddTask.ClearLookAt();
			ts.Close();
			await Game.Player.Character.RunTaskSequence(ts);

			API.SetScenarioTypeEnabled("PROP_HUMAN_ATM", true);
			API.ResetScenarioTypesEnabled();
			API.TaskStartScenarioInPlace(Game.PlayerPed.Handle, "PROP_HUMAN_ATM", 0, true);
			this.inAnimating = true;

			//bool result = await Rpc.Server
			//	.Event(RpcEvents.BankAtmWithdraw)
			//	.Attach(atm.Item1.Id)
			//	.Attach(Guid.Parse("e9286e6f-e74d-4510-855b-5318ef0f71af"))
			//	.Attach(100)
			//	.Request<bool>();

			//this.Logger.Log($"ATM Withdraw response: {result}");
		}
	}
}
