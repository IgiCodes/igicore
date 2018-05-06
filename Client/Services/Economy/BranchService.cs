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
using IgiCore.Core;
using IgiCore.Core.Extensions;
using IgiCore.Core.Models.Economy.Banking;

namespace IgiCore.Client.Services.Economy
{
    public class BranchService : ClientService
    {
        protected bool CharLoaded = false;
        protected bool InAnim = false;
        protected List<BankBranch> Branches;
        protected Dictionary<BankBranch, Ped> Tellers = new Dictionary<BankBranch, Ped>();

        public BranchService()
        {
            Client.Instance.OnClientReady += OnClientReady;
            Client.Instance.OnCharacterLoaded += OnCharacterLoaded;
        }

        private async void OnClientReady(object s, ServerInformationEventArgs a) { this.Branches = a.Information.Branches.ToList(); }

        private async void OnCharacterLoaded(object s, CharacterEventArgs c) { this.CharLoaded = true; }

        public override async Task Tick()
        {
            if (!this.CharLoaded) return;
            if (this.InAnim && Input.Input.IsControlJustPressed(Control.MoveUpOnly))
            {
                Game.Player.Character.Task.ClearAllImmediately(); // Cancel animation
                this.InAnim = false;
            }
            if (Game.Player.Character.IsInVehicle() || this.InAnim) return;

            foreach (BankBranch bankBranch in this.Branches)
            {
                if (this.Tellers.ContainsKey(bankBranch) && this.Tellers[bankBranch].Handle != 0) continue;
                var tellerModel = new Model(PedHash.Bankman);
                await tellerModel.Request(-1);
                this.Tellers[bankBranch] = await CitizenFX.Core.World.CreatePed(tellerModel, bankBranch.Position, bankBranch.Heading);
                this.Tellers[bankBranch].Task?.ClearAllImmediately();
                this.Tellers[bankBranch].Task?.StandStill(1);
                this.Tellers[bankBranch].AlwaysKeepTask = true;
                this.Tellers[bankBranch].IsInvincible = true;
                this.Tellers[bankBranch].IsPositionFrozen = true;
                this.Tellers[bankBranch].BlockPermanentEvents = true;
            }

            //foreach (var banker in this.Tellers.Select(x=>x.Value))
            //{
            //    CitizenFX.Core.World.DrawMarker(MarkerType.HorizontalCircleSkinny, banker.GetPositionInFront(1.5f), Vector3.Zero, Vector3.Zero, Vector3.One * 2, Color.FromArgb(50, 239, 239, 239));
            //}

            KeyValuePair<BankBranch, Ped> teller = this.Tellers
                .Select(t => new {teller = t, distance = t.Value?.Position.DistanceToSquared(Game.Player.Character.Position) ?? float.MaxValue})
                .Where(t => t.distance < 5.0F) // Nearby
                //.Where(a => Vector3.Dot(a.Item2.ForwardVector, Vector3.Normalize(a.Item2.Position - Game.Player.Character.Position)).IsBetween(0f, 1.0f)) // In front of
                .OrderBy(t => t.distance)
                .Select(t => t.teller)
                .FirstOrDefault();

            if (teller.Value == null) return;

            new Text($"Press M to use Branch {teller.Key.Name}", new PointF(50, Screen.Height - 50), 0.4f, Color.FromArgb(255, 255, 255), Font.ChaletLondon, Alignment.Left, false, true).Draw();

            if (!Input.Input.IsControlJustPressed(Control.InteractionMenu)) return;

            TaskSequence ts = new TaskSequence();
            ts.AddTask.LookAt(teller.Value);
            //ts.AddTask.GoTo(teller.Value, teller.Value.Position - teller.Value.Position.TranslateDir(teller.Value.Heading - 90, 1.5f), 2000);
            ts.AddTask.GoTo(teller.Value.GetPositionInFront(1.5f));
            ts.AddTask.AchieveHeading(teller.Value.Heading - 180);
            ts.AddTask.ClearLookAt();
            ts.Close();
            Game.Player.Character.Task.PerformSequence(ts);

        }
    }
}
