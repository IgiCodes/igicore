using IgiCore.SDK.Core.Diagnostics;
using IgiCore.SDK.Server.Rpc;
using JetBrains.Annotations;

namespace IgiCore.SDK.Server.Controllers
{
	[PublicAPI]
	public abstract class Controller
	{
		protected readonly ILogger Logger;
		protected readonly IRpcHandler Rpc;

		protected Controller(ILogger logger, IRpcHandler rpc)
		{
			this.Logger = logger;
			this.Rpc = rpc;
		}
	}
}
