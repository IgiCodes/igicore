namespace IgiCore.SDK.Server.Rpc
{
	public interface IClient
	{
		int Handle { get; set; }

		string Name { get; }

		int Ping { get; }

		IRpcTrigger Event(string @event);
	}
}
