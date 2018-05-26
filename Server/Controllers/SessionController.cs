using System;
using System.Collections.Generic;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using IgiCore.SDK.Core.Diagnostics;
using IgiCore.SDK.Server;
using IgiCore.Server.Events;

namespace IgiCore.Server.Controllers
{
	public class SessionController : Controller
	{
		private readonly List<Action> callbacks = new List<Action>();
		
		public Player CurrentHost { get; private set; }

		public SessionController(ILogger logger, EventsManager events) : base(logger, events)
		{
			API.EnableEnhancedHostSupport(true);

			//events.Event("hostingSession").On(OnHostingSession);
			//events.Event("HostedSession").On(OnHostedSession);
		}
		
		private async void OnHostingSession([FromSource] Player player)
		{
			if (this.CurrentHost != null)
			{
				player.TriggerEvent("sessionHostResult", "wait");

				this.callbacks.Add(() => player.TriggerEvent("sessionHostResult", "free"));

				return;
			}

			string hostId;

			try
			{
				hostId = API.GetHostId();
			}
			catch (NullReferenceException)
			{
				hostId = null;
			}

			if (!string.IsNullOrEmpty(hostId) && API.GetPlayerLastMsg(API.GetHostId()) < 1000)
			{
				player.TriggerEvent("sessionHostResult", "conflict");

				return;
			}

			this.callbacks.Clear();
			this.CurrentHost = player;

			this.Logger.Log($"Game host is now {this.CurrentHost.Handle} \"{this.CurrentHost.Name}\"");

			player.TriggerEvent("sessionHostResult", "go");

			await BaseScript.Delay(5000);

			this.callbacks.ForEach(c => c());
			this.CurrentHost = null;
		}

		private void OnHostedSession([FromSource] Player player)
		{
			if (this.CurrentHost != null && this.CurrentHost != player) return;

			this.callbacks.ForEach(c => c());
			this.CurrentHost = null;
		}
	}
}
