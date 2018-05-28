namespace IgiCore.SDK.Server.Rpc
{
	public interface IClient
	{
		int Handle { get; }

		string Name { get; }

		long SteamId { get; }

		string EndPoint { get; }

		int Ping { get; }

		IRpcTrigger Event(string @event);
	}
}
