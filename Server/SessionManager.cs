using System;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace IgiCore.Server
{
	public static class SessionManager
	{
		private static readonly List<Action> Callbacks = new List<Action>();

		public static Player CurrentHost { get; private set; }
		
		static SessionManager()
		{
			//Server.Log("Starting Session Manager");

			EnableEnhancedHostSupport(true);
		}

		public static async void OnHostingSession([FromSource] Player player)
		{
			//Server.Log($"OnHostingSession: {player.Handle}");

			if (CurrentHost != null)
			{
				//Server.Log("sessionHostResult wait");
				BaseScript.TriggerClientEvent(player, "sessionHostResult", "wait");

				Callbacks.Add(() =>
				{
					//Server.Log("sessionHostResult free");
					BaseScript.TriggerClientEvent(player, "sessionHostResult", "free");
				});

				return;
			}

			string hostId;
			
			try
			{
				hostId = GetHostId();
			}
			catch (NullReferenceException)
			{
				hostId = null;
			}

			if (!string.IsNullOrEmpty(hostId) && GetPlayerLastMsg(GetHostId()) < 1000)
			{
				//Server.Log("sessionHostResult conflict");
				BaseScript.TriggerClientEvent(player, "sessionHostResult", "conflict");

				return;
			}

			Callbacks.Clear();
			CurrentHost = player;

			Server.Log($"[SESSIONMANAGER] Game host is now \"{CurrentHost.Name}\"");

			//Server.Log("sessionHostResult go");
			BaseScript.TriggerClientEvent(player, "sessionHostResult", "go");

			await BaseScript.Delay(5000);

			Callbacks.ForEach(c => c());
			CurrentHost = null;
		}

		public static void OnHostedSession([FromSource] Player player)
		{
			//Server.Log($"OnHostedSession: {player.Handle}");

			if (CurrentHost != null && CurrentHost != player) return;

			Callbacks.ForEach(c => c());
			CurrentHost = null;
		}
	}
}
