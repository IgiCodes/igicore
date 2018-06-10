using IgiCore.SDK.Core.Diagnostics;
using IgiCore.SDK.Server.Controllers;
using IgiCore.SDK.Server.Events;
using IgiCore.SDK.Server.Rpc;

namespace Roleplay.Server.Controllers
{
	public class RoleplayController : ConfigurableController<Configuration>
	{
		public RoleplayController(ILogger logger, IEventManager events, IRpcHandler rpc, Configuration configuration) : base(logger, events, rpc, configuration)
		{
			this.Logger.Debug(this.Configuration.Test);
		}
	}
}
