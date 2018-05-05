using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using IgiCore.Client.Events;
using IgiCore.Core;
using IgiCore.Core.Extensions;
using IgiCore.Core.Models.Economy.Banking;

namespace IgiCore.Client.Services.Economy
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

            var i = 1;
            foreach (BankAtm bankAtm in this.Atms)
            {
                CitizenFX.Core.World.DrawMarker(MarkerType.HorizontalCircleSkinny, bankAtm.Position, Vector3.Zero, Vector3.Zero, Vector3.One * 5, Color.FromArgb(50, 239, 239, 239));
                new Text($"{Vector3.Dot(Game.Player.Character.ForwardVector, Vector3.Normalize(bankAtm.Position - Game.Player.Character.Position))}", new PointF(50, Screen.Height - (i * 50) - 50), 0.4f, Color.FromArgb(255, 255, 255), Font.ChaletLondon, Alignment.Left, false, true).Draw();
                i++;
            }



            Tuple<BankAtm, Prop> atm = this.Atms
                .Where(a => a.Position.DistanceToSquared(Game.Player.Character.Position) < 5.0F) // Nearby
                .Select(a => new Tuple<BankAtm, Prop>(a, new Prop(API.GetClosestObjectOfType(a.PosX, a.PosY, a.PosZ, 1, a.Hash, false, false, false))))
                .Where(p => p.Item2.Model.IsValid)
                .Where(a => Vector3.Dot(a.Item2.ForwardVector, Vector3.Normalize(a.Item2.Position - Game.Player.Character.Position)).IsBetween(0f, 1.0f)) // In front of
                .OrderBy(a => a.Item2.Position.DistanceToSquared(Game.Player.Character.Position))
                .FirstOrDefault();

            if (atm == null) return;

            new Text("Press M to use ATM", new PointF(50, Screen.Height - 50), 0.4f, Color.FromArgb(255, 255, 255), Font.ChaletLondon, Alignment.Left, false, true).Draw();

            if (!Input.Input.IsControlJustPressed(Control.InteractionMenu)) return;

            Game.Player.Character.Task.GoTo(atm.Item2, Vector3.Zero, 2000); // Need to provide an offset or the player tried to walk inside the model
            await BaseScript.Delay(2000);
            Game.Player.Character.Task.TurnTo(atm.Item2, 1500);
            await BaseScript.Delay(1500);

            API.SetScenarioTypeEnabled("PROP_HUMAN_ATM", true);
            API.ResetScenarioTypesEnabled();
            API.TaskStartScenarioInPlace(Game.PlayerPed.Handle, "PROP_HUMAN_ATM", 0, true);
            this.InAnim = true;

            bool result = await Rpc.Server.Request<Guid, Guid, double, bool>(
                RpcEvents.BankAtmWithdraw,
                atm.Item1.Id,
                Guid.Parse("e9286e6f-e74d-4510-855b-5318ef0f71af"),
                100
            );

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
