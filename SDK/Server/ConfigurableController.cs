using IgiCore.SDK.Core;
using IgiCore.SDK.Core.Diagnostics;
using IgiCore.SDK.Server.Rpc;

namespace IgiCore.SDK.Server
{
	public abstract class ConfigurableController<T> : Controller where T : IControllerConfiguration
	{
		public T Configuration { get; set; }

		protected ConfigurableController(ILogger logger, IEventsManager events, T configuration) : base(logger, events)
		{
			this.Configuration = configuration;
		}
	}
}
