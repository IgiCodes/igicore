using CitizenFX.Core;
using IgiCore.SDK.Server.Rpc;
using IgiCore.Server.Diagnostics;

namespace IgiCore.Server.Rpc
{
	public static class RpcManager
	{
		private static readonly Logger Logger;
		private static readonly Serializer Serializer;
		private static readonly ClientTrigger Trigger;
		private static ClientHandler handler;

		static RpcManager()
		{
			Logger = new Logger("RPC");
			Serializer = new Serializer();
			Trigger = new ClientTrigger(Logger, Serializer);
		}

		public static void Configure(EventHandlerDictionary events)
		{
			handler = new ClientHandler(events);
		}

		public static IRpc Event(string @event) => new Rpc(@event, Logger, handler, Trigger, Serializer);
	}
}
