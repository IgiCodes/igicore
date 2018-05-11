namespace IgiCore.Core.Rpc
{
	public interface IRpcTrigger
	{
		void Fire(RpcMessage message);
	}
}
