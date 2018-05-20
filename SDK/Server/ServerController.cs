using IgiCore.SDK.Core.Diagnostics;
using IgiCore.SDK.Server.Configuration;
using IgiCore.SDK.Server.Rpc;

namespace IgiCore.SDK.Server
{
	public abstract class ServerController
	{
		protected readonly ILogger Logger;
		protected readonly IServerEventsManager ServerEvents;
		protected readonly IClientEventsManager ClientEvents;

		public ServerController(ILogger logger, IServerEventsManager serverEvents, IClientEventsManager clientEvents, IConfiguration configuration)
		{
			this.Logger = logger;
			this.ServerEvents = serverEvents;
			this.ClientEvents = clientEvents;

			ServerConfiguration.Load(configuration);
		}

		public virtual void Initialize()
		{
		}
	}
}
