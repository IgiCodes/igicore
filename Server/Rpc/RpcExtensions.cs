using CitizenFX.Core;

namespace IgiCore.Server.Rpc
{
	public static class RpcExtensions
	{
		public static RpcRequest Event(this Player player, string @event)
		{
			return new RpcRequest(@event, new RpcHandler(), new RpcTrigger(), new RpcSerializer()).Target(player);
		}
	}
}
