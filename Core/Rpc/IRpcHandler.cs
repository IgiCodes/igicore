using System;

namespace IgiCore.Core.Rpc
{
	public interface IRpcHandler
	{
		void Attach(string @event, Delegate callback);
		void Detach(string @event, Delegate callback);
	}
}
