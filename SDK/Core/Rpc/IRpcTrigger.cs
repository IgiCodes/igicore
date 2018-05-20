namespace IgiCore.SDK.Core.Rpc
{
	public interface IRpcTrigger
	{
		void Fire(RpcMessage message);
	}
}
