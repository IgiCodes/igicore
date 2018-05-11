namespace IgiCore.Client.Rpc
{
	public static class Server
	{
		public static RpcRequest Event(string @event)
		{
			return new RpcRequest(@event, new RpcHandler(), new RpcTrigger(), new RpcSerializer());
		}
	}
}
