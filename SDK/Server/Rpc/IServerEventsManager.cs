using System;

namespace IgiCore.SDK.Server.Rpc
{
	public interface IServerEventsManager
	{
		void Trigger(string @event, params object[] args);

		void On(string @event, Delegate callback);
	}
}
