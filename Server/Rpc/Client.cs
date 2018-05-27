using IgiCore.SDK.Server.Rpc;

namespace IgiCore.Server.Rpc
{
	public class Client : IClient
	{
		public int Handle { get; set; }

		public string Name { get; }

		public int Ping { get; }

		public IRpcTrigger Event(string @event) => RpcManager.Event(@event);
	}
}
