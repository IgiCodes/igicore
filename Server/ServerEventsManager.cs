using System;
using System.Collections.Generic;
using IgiCore.Models.Player;
using IgiCore.SDK.Server.Rpc;

namespace IgiCore.Server
{
	public class ServerEventsManager : IServerEventsManager
	{
		private readonly Dictionary<string, List<Delegate>> events = new Dictionary<string, List<Delegate>>();

		public void Trigger(string @event, params object[] args)
		{
			if (!this.events.ContainsKey(@event)) return;

			foreach (var callback in this.events[@event])
			{
				callback.DynamicInvoke(args);
			}
		}

		public void On(string @event, Delegate callback)
		{
			if (!this.events.ContainsKey(@event))
			{
				this.events.Add(@event, new List<Delegate>());
			}

			this.events[@event].Add(callback);
		}
	}
}
