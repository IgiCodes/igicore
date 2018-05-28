using IgiCore.SDK.Core.Diagnostics;
using IgiCore.SDK.Server.Events;
using IgiCore.SDK.Server.Rpc;
using JetBrains.Annotations;

namespace IgiCore.SDK.Server.Controllers
{
	[PublicAPI]
	public abstract class Controller
	{
		protected readonly ILogger Logger;
		protected readonly IEventManager Events;
		protected readonly IRpcHandler Rpc;

		protected Controller(ILogger logger, IEventManager events, IRpcHandler rpc)
		{
			this.Logger = logger;
			this.Events = events;
			this.Rpc = rpc;
		}
	}
}
