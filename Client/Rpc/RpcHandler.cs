using IgiCore.SDK.Client.Rpc;

namespace IgiCore.Client.Rpc
{
	public class RpcHandler : IRpcHandler
	{
		public IRpc Event(string @event) => RpcManager.Event(@event);
	}
}
