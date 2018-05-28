using IgiCore.SDK.Core.Controllers;
using IgiCore.SDK.Core.Diagnostics;
using IgiCore.SDK.Server.Events;
using IgiCore.SDK.Server.Rpc;

namespace IgiCore.SDK.Server.Controllers
{
	public abstract class ConfigurableController<T> : Controller where T : IControllerConfiguration
	{
		protected readonly T Configuration;

		protected ConfigurableController(ILogger logger, IEventManager events, IRpcHandler rpc, T configuration) : base(logger, events, rpc)
		{
			this.Configuration = configuration;
		}
	}
}
