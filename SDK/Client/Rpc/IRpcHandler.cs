namespace IgiCore.SDK.Client.Rpc
{
	public interface IRpcHandler
	{
		IRpc Event(string @event);
	}
}
