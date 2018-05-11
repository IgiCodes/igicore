using System;
using System.Collections.Generic;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace IgiCore.Server.Managers
{
	public static class SessionManager
	{
		private static readonly List<Action> Callbacks = new List<Action>();

		public static Player CurrentHost { get; private set; }

		static SessionManager()
		{
			API.EnableEnhancedHostSupport(true);
		}

		public static async void OnHostingSession([FromSource] Player player)
		{
			if (CurrentHost != null)
			{
				player.TriggerEvent("sessionHostResult", "wait");

				Callbacks.Add(() => player.TriggerEvent("sessionHostResult", "free"));

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

			Callbacks.Clear();
			CurrentHost = player;

			Server.Log($"[SESSIONMANAGER] Game host is now \"{CurrentHost.Name}\"");

			player.TriggerEvent("sessionHostResult", "go");

			await BaseScript.Delay(5000);

			Callbacks.ForEach(c => c());
			CurrentHost = null;
		}

		public static void OnHostedSession([FromSource] Player player)
		{
			if (CurrentHost != null && CurrentHost != player) return;

			Callbacks.ForEach(c => c());
			CurrentHost = null;
		}
	}
}
