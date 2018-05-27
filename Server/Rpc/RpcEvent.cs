using IgiCore.SDK.Server.Rpc;

namespace IgiCore.Server.Rpc
{
	public class RpcEvent : IRpcEvent
	{
		public string Event { get; set; }

		public IClient Client { get; set; }

		public void Reply(params object[] payloads)
		{
			this.Client.Event(this.Event).Trigger(payloads);
		}
	}
}
