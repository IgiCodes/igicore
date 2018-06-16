using IgiCore.SDK.Core.Diagnostics;
using IgiCore.SDK.Server.Controllers;
using IgiCore.SDK.Server.Events;
using IgiCore.SDK.Server.Rpc;

namespace TemplatePlugin.Server.Controllers
{
	public class TemplateController : ConfigurableController<Configuration>
	{
		public TemplateController(ILogger logger, IEventManager events, IRpcHandler rpc, Configuration configuration) : base(logger, events, rpc, configuration)
		{
			this.Logger.Debug(this.Configuration.Test);
		}
	}
}
