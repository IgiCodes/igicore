using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Banking.Core.Models;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using IgiCore.SDK.Client;
using IgiCore.SDK.Client.Extensions;
using IgiCore.SDK.Client.Input;
using IgiCore.SDK.Client.Rpc;
using IgiCore.SDK.Core.Diagnostics;
using IgiCore.SDK.Core.Helpers;

namespace Banking.Client
{
    public class BranchService : Service
	{
        protected bool CharLoaded = false;
        protected bool InAnim = false;
        protected Camera Camera;
        protected List<BankBranch> Branches;
        protected Dictionary<BankBranch, Ped> Tellers = new Dictionary<BankBranch, Ped>();

        public BranchService(ILogger logger, IEventsManager events) : base(logger, events)
		{
	        //Client.Instance.Controllers.First<ClientController>().OnClientReady += OnClientReady;
            //Client.Instance.Controllers.First<CharacterController>().OnCharacterLoaded += OnCharacterLoaded;
        }

        //private async void OnClientReady(object s, ServerInformationEventArgs a) { this.Branches = a.Information.Branches.ToList(); }

        //private async void OnCharacterLoaded(object s, CharacterEventArgs c) { this.CharLoaded = true; }

        public override async Task Tick()
        {
            if (!this.CharLoaded) return;

            foreach (BankBranch bankBranch in this.Branches)
            {
                if (this.Tellers.ContainsKey(bankBranch) && this.Tellers[bankBranch].Handle != 0)
                {
                   // this.Tellers[bankBranch].Position = bankBranch.Position;
                    this.Tellers[bankBranch].Heading = bankBranch.Heading;
                    continue;
                }
                var tellerModel = new Model(PedHash.Bankman);
                await tellerModel.Request(-1);
                //this.Tellers[bankBranch] = await CitizenFX.Core.World.CreatePed(tellerModel, bankBranch.Position, bankBranch.Heading);
                this.Tellers[bankBranch].Task?.ClearAllImmediately();
                this.Tellers[bankBranch].Task?.StandStill(1);
                this.Tellers[bankBranch].AlwaysKeepTask = true;
                this.Tellers[bankBranch].IsInvincible = true;
                this.Tellers[bankBranch].IsPositionFrozen = true;
                this.Tellers[bankBranch].BlockPermanentEvents = true;
                this.Tellers[bankBranch].IsCollisionProof = false;
            }

            if (this.InAnim && Input.IsControlJustPressed(Control.MoveUpOnly))
            {
                Game.Player.Character.Task.ClearAll();
                Game.Player.Character.Task.ClearLookAt();
                CitizenFX.Core.World.DestroyAllCameras();
                this.Camera = null;
                CitizenFX.Core.World.RenderingCamera = null;
                this.InAnim = false;
            }

            if (Game.Player.Character.IsInVehicle() || this.InAnim) return;

            //foreach (var banker in this.Tellers.Select(x=>x.Value))
            //{
            //    CitizenFX.Core.World.DrawMarker(MarkerType.HorizontalCircleSkinny, banker.GetPositionInFront(1.5f), Vector3.Zero, Vector3.Zero, Vector3.One * 2, Color.FromArgb(50, 239, 239, 239));
            //}

            KeyValuePair<BankBranch, Ped> teller = this.Tellers
                .Select(t => new {teller = t, distance = t.Value?.Position.DistanceToSquared(Game.Player.Character.Position) ?? float.MaxValue})
                .Where(t => t.distance < 5.0F) // Nearby
                //.Where(a => Vector3.Dot(a.Item2.ForwardVector, Vector3.Normalize(a.Item2.Position - Game.Player.ActiveCharacter.Position)).IsBetween(0f, 1.0f)) // In front of
                .OrderBy(t => t.distance)
                .Select(t => t.teller)
                .FirstOrDefault();

            if (teller.Value == null) return;

            new Text($"Press M to use Branch {teller.Key.Name}", new PointF(50, Screen.Height - 50), 0.4f, Color.FromArgb(255, 255, 255), Font.ChaletLondon, Alignment.Left, false, true).Draw();

            if (!Input.IsControlJustPressed(Control.InteractionMenu)) return;

            this.InAnim = true;

            Ped bankTeller = teller.Value;

            TaskSequence ts = new TaskSequence();
            ts.AddTask.LookAt(bankTeller);
            ts.AddTask.GoTo(bankTeller.GetPositionInFront(1.5f));
            ts.AddTask.AchieveHeading(bankTeller.Heading - 180);
            ts.Close();
            await Game.Player.Character.RunTaskSequence(ts);
            this.Logger.Log("Task ended");
            Game.Player.Character.Task.LookAt(bankTeller);
            Game.Player.Character.Task.StandStill(-1);

            // Camera
            if (this.Camera == null) this.Camera = CitizenFX.Core.World.CreateCamera(GameplayCamera.Position, GameplayCamera.Rotation, GameplayCamera.FieldOfView);
            CitizenFX.Core.World.RenderingCamera = this.Camera;

            for (float t=0; t<1f; t+=0.01f)
            {
                var interval = (float)(t < 0.5 ? 2.0 * t * t : -2.0 * t * t + 4.0 * t - 1.0);
                this.Camera.Position = VectorExtensions.Lerp(
                    GameplayCamera.Position,
                    teller.Value.Position.TranslateDir(bankTeller.Heading + 110, 2.2f) + Vector3.UnitZ * 0.8f,
                    interval
                );
                this.Camera.PointAt(VectorExtensions.Lerp(Game.PlayerPed.Position, bankTeller.Position, interval) + Vector3.UnitZ * 0.4f);
                this.Camera.FieldOfView = MathHelpers.Lerp(GameplayCamera.FieldOfView, 30, interval);
                await BaseScript.Delay(1);
            }
        }
    }
}
