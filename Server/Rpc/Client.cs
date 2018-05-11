namespace IgiCore.Server.Rpc
{
	public static class Client
	{
		public static RpcRequest Event(string @event)
		{
			return new RpcRequest(@event, new RpcHandler(), new RpcTrigger(), new RpcSerializer());
		}
	}
}
