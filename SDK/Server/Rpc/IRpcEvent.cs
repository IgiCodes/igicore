using IgiCore.SDK.Core.Models.Player;

namespace IgiCore.SDK.Server.Rpc
{
	public interface IRpcEvent
	{
		string Event { get; }

		IClient Client { get; }

		User User { get; }

		void Reply(params object[] payloads);
	}
}
