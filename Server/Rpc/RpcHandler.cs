using IgiCore.SDK.Server.Rpc;

namespace IgiCore.Server.Rpc
{
	public class RpcHandler : IRpcHandler
	{
		public IRpc Event(string @event) => RpcManager.Event(@event);
	}
}
