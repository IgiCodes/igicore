using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using IgiCore.Client.Events;
using IgiCore.Client.Extensions;
using IgiCore.Core.Extensions;
using IgiCore.Core.Models.Economy.Banking;
using IgiCore.Core.Rpc;

namespace IgiCore.Client.Services.Economy.Banking
{
    public class AtmService : ClientService
    {
        protected bool InAnim = false;
        protected List<BankAtm> Atms = new List<BankAtm>();

        public AtmService()
        {
            Client.Instance.OnClientReady += OnClientReady;
        }

        private async void OnClientReady(object s, ServerInformationEventArgs a) { this.Atms = a.Information.Atms.ToList(); }


        public readonly List<int> Models = new List<int>
        {
            506770882, // prop_fleeca_atm
		    -870868698, // prop_atm_01
		    -1126237515, // prop_atm_02
		    -1364697528, // prop_atm_03
        };

        public override async Task Tick()
        {
            if (this.InAnim && Input.Input.IsControlJustPressed(Control.MoveUpOnly))
            {
                Game.Player.Character.Task.ClearAllImmediately(); // Cancel animation
                this.InAnim = false;
            }
            if (Game.Player.Character.IsInVehicle() || this.InAnim) return;

            Tuple<BankAtm, Prop> atm = this.Atms
                .Select(a => new { atm = a, distance = a.Position.DistanceToSquared(Game.Player.Character.Position) })
                .Where(a => a.distance < 5.0F) // Nearby
                .Select(a => new { a.atm, prop = new Prop(API.GetClosestObjectOfType(a.atm.PosX, a.atm.PosY, a.atm.PosZ, 1, (uint)a.atm.Hash, false, false, false)), a.distance })
                .Where(p => p.prop.Model.IsValid)
                .Where(a => Vector3.Dot(a.prop.ForwardVector, Vector3.Normalize(a.prop.Position - Game.Player.Character.Position)).IsBetween(0f, 1.0f)) // In front of
                .OrderBy(a => a.distance)
                .Select(a => new Tuple<BankAtm, Prop>(a.atm, a.prop))
                .FirstOrDefault();

            //var i = 0;
            //foreach (var tatm in this.Atms)
            //{
            //    i++;
            //    var prop = new Prop(API.GetClosestObjectOfType(tatm.PosX, tatm.PosY, tatm.PosZ, 1, (uint)tatm.Hash, false, false, false));
            //    CitizenFX.Core.World.DrawMarker(MarkerType.HorizontalCircleSkinny, prop.Position - (prop.Position - prop.Position.TranslateDir(prop.Heading - 90, 0.4f)) + Vector3.Up * 0.1f, Vector3.Zero, Vector3.Zero, Vector3.One * 2, Color.FromArgb(50, 239, 239, 239));
            //}

            if (atm == null) return;


            new Text("Press M to use ATM", new PointF(50, Screen.Height - 50), 0.4f, Color.FromArgb(255, 255, 255), Font.ChaletLondon, Alignment.Left, false, true).Draw();

            if (!Input.Input.IsControlJustPressed(Control.InteractionMenu)) return;

            TaskSequence ts = new TaskSequence();
            ts.AddTask.LookAt(atm.Item2);
            //ts.AddTask.GoTo(atm.Item2, atm.Item2.Position.TranslateDir(atm.Item2.Heading - 90, 0.4f) - atm.Item2.Position, 2000);
            ts.AddTask.GoTo(atm.Item2, Vector3.Zero, 2000);
            ts.AddTask.AchieveHeading(atm.Item2.Heading);
            ts.AddTask.ClearLookAt();
            ts.Close();
            await Game.Player.Character.RunTaskSequence(ts);

            API.SetScenarioTypeEnabled("PROP_HUMAN_ATM", true);
            API.ResetScenarioTypesEnabled();
            API.TaskStartScenarioInPlace(Game.PlayerPed.Handle, "PROP_HUMAN_ATM", 0, true);
            this.InAnim = true;

            bool result = await Rpc.Server
	            .Event(RpcEvents.BankAtmWithdraw)
				.Attach(atm.Item1.Id)
				.Attach(Guid.Parse("e9286e6f-e74d-4510-855b-5318ef0f71af"))
				.Attach(100)
	            .Request<bool>();

            Client.Log($"ATM Withdraw response: {result}");


            // TODO: Better?
            //Game.Player.Character.Task.PlayAnimation("amb@prop_human_atm@male@enter", "enter");
            //Game.Player.Character.Task.PlayAnimation("amb@prop_human_atm@male@base", "base");
            //Game.Player.Character.Task.PlayAnimation("amb@prop_human_atm@male@idle_a", "idle_a");
            //Game.Player.Character.Task.PlayAnimation("amb@prop_human_atm@male@idle_a", "idle_b");
            //Game.Player.Character.Task.PlayAnimation("amb@prop_human_atm@male@exit", "exit");
        }
    }
}
