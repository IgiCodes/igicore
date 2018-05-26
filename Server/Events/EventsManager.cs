using CitizenFX.Core;
using IgiCore.SDK.Server.Rpc;
using IgiCore.Server.Diagnostics;
using IgiCore.Server.Rpc;

namespace IgiCore.Server.Events
{
	public class EventsManager : IEventsManager
	{
		private readonly Logger logger;
		private readonly ClientHandler handler;
		private readonly Serializer serializer;
		private readonly ClientTrigger trigger;

		public EventsManager(Logger logger, EventHandlerDictionary events)
		{
			this.logger = logger;
			this.handler = new ClientHandler(events);
			this.serializer = new Serializer();
			this.trigger = new ClientTrigger(this.logger, this.serializer);
		}

		//public ClientEvent this[string @event] => new ClientEvent(@event, this.logger, this.handler, this.trigger, this.serializer);

		public IClientEvent Event(string @event)
		{
			return new ClientEvent(@event, this.logger, this.handler, this.trigger, this.serializer);
		}
	}
}
