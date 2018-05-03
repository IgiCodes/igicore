using System;
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
    public class ATMService : ClientService
    {
        protected bool enabled;

        public event EventHandler<EventArgs> OnEnabled;

        public bool Enabled
        {
            get => this.enabled;
            set
            {
                this.enabled = value;

                this.OnEnabled?.Invoke(this, EventArgs.Empty);
            }
        }

        public override async Task Tick()
        {

            foreach (var atm in new ObjectList().Where(o => o.Model.IsValid && o.Model.Hash == 506770882)) // prop_fleeca_atm
            {
                Vector3 atmToPlayer = atm.Position - Game.Player.Character.Position;
                atmToPlayer.Normalize();
                float atmToPlayerDot = Vector3.Dot(Game.Player.Character.ForwardVector, atmToPlayer);

                if (Vector3.Distance(Game.Player.Character.Position, atm.Position).IsBetween(1.0f, 1.8f) && Game.Player.Character.ForwardVector.DistanceToSquared(atm.ForwardVector) < 1.0f && atmToPlayerDot.IsBetween(0.3f, 0.8f))
                {
                    new Text($"Allow ATM Access for {atm.Handle}", new PointF(230, Screen.Height - 100), 0.4f, Color.FromArgb(255, 255, 255), Font.ChaletLondon, Alignment.Left, false, true).Draw();
                }
            }

            if (this.Enabled) return;
        }
    }
}
