using IgiCore.SDK.Client.Rpc;

namespace IgiCore.Client.Rpc
{
	public class RpcEvent : IRpcEvent
	{
		public string Event { get; set; }

		public void Reply(params object[] payloads)
		{
			// TODO
		}
	}
}
