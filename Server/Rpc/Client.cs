using System.Globalization;
using CitizenFX.Core;
using IgiCore.SDK.Server.Rpc;

namespace IgiCore.Server.Rpc
{
	public class Client : IClient
	{
		public int Handle { get; }

		public string Name { get; }

		public long SteamId { get; }

		public string EndPoint { get; }

		public int Ping { get; }

		public Client(int handle)
		{
			this.Handle = handle;

			var player = new PlayerList()[this.Handle];

			this.Name = player.Name;
			this.SteamId = long.Parse(player.Identifiers["steam"], NumberStyles.HexNumber);
			this.EndPoint = player.EndPoint;
			this.Ping = player.Ping;
		}

		public IRpcTrigger Event(string @event) => RpcManager.Event(@event);
	}
}
