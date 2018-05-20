using IgiCore.SDK.Core.Diagnostics;
using IgiCore.Server.Rpc;

namespace IgiCore.Server.Controllers
{
	public class ResourceController : Controller
	{
		public ResourceController(ILogger logger) : base(logger) { }

		public override void Initialize()
		{
			Client.Event(ServerEvents.ResourceStarting).On<string>(r => this.Logger.Log($"Starting resource: {r}"));
			Client.Event(ServerEvents.ResourceStart).On<string>(r => this.Logger.Log($"Start resource: {r}"));
			Client.Event(ServerEvents.ResourceStop).On<string>(r => this.Logger.Log($"Stop resource: {r}"));
		}
	}
}
