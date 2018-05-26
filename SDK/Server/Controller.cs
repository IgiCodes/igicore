using IgiCore.SDK.Core.Diagnostics;
using IgiCore.SDK.Server.Rpc;
using JetBrains.Annotations;

namespace IgiCore.SDK.Server
{
	[PublicAPI]
	public abstract class Controller
	{
		protected readonly ILogger Logger;
		protected readonly IEventsManager Events;

		protected Controller(ILogger logger, IEventsManager events)
		{
			this.Logger = logger;
			this.Events = events;
		}
	}
}
