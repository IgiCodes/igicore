namespace IgiCore.SDK.Server.Rpc
{
	public interface IRpcEvent
	{
		string Event { get; }

		IClient Client { get; set; }

		void Reply(params object[] payloads);
	}
}
