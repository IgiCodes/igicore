using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using IgiCore.Client.Extensions;
using IgiCore.Core.Extensions;

namespace IgiCore.Client.Services.Player
{
	public class PlayerIdleService : ClientService
	{
		public event EventHandler<EventArgs> OnIdle;

		public TimeSpan Timeout { get; set; } = TimeSpan.FromMinutes(15);
		public TimeSpan TimeoutWarning { get; set; } = TimeSpan.FromMinutes(10);
		public int LastActive { get; protected set; } = Game.GameTime;
		public TimeSpan IdleFor => TimeSpan.FromMilliseconds(Game.GameTime - this.LastActive);

		public override async Task Tick()
		{
			if (Input.Input.IsAnyControlJustPressed()) this.LastActive = Game.GameTime;
		
			//new Text(this.IdleFor.TotalSeconds.ToString("N"), new PointF(20, 10), 0.5f).Draw();

			if (this.IdleFor < this.TimeoutWarning) return;
			
			if (this.IdleFor < this.Timeout)
			{
				Screen.ShowNotification($"You will be kicked for being AFK in {"minute".Pluralize(Math.Ceiling(this.Timeout.Subtract(this.IdleFor).TotalMinutes))}");
			}
			else
			{
				Screen.ShowNotification("You are now AFK");

				this.OnIdle?.Invoke(this, EventArgs.Empty);
			}
		}
	}
}
