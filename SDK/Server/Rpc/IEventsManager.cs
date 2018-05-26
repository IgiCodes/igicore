namespace IgiCore.SDK.Server.Rpc
{
	public interface IEventsManager
	{
		IClientEvent Event(string @event);
	}
}
