namespace IgiCore.SDK.Server.Rpc
{
	public interface IRpcHandler
	{
		IRpc Event(string @event);
	}
}
