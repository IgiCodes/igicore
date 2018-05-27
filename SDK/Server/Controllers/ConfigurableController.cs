using IgiCore.SDK.Core.Controllers;
using IgiCore.SDK.Core.Diagnostics;
using IgiCore.SDK.Server.Rpc;

namespace IgiCore.SDK.Server.Controllers
{
	public abstract class ConfigurableController<T> : Controller where T : IControllerConfiguration
	{
		protected readonly T Configuration;

		protected ConfigurableController(ILogger logger, IRpcHandler rpc, T configuration) : base(logger, rpc)
		{
			this.Configuration = configuration;
		}
	}
}
