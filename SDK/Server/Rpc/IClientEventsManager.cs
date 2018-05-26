using System;

namespace IgiCore.SDK.Server.Rpc
{
	public interface IClientEventsManager
	{
		void On(string @event, Delegate callback);
	}
}
