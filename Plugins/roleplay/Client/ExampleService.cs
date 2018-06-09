using System.Threading.Tasks;
using IgiCore.SDK.Client.Events;
using IgiCore.SDK.Client.Rpc;
using IgiCore.SDK.Client.Services;
using IgiCore.SDK.Core.Diagnostics;

namespace Roleplay.Client
{
	public class ExampleService : Service
	{
		public ExampleService(ILogger logger, ITickManager ticks, IEventManager events, IRpcHandler rpc) : base(logger, ticks, events, rpc)
		{
			this.Ticks.Attach(Tick);
		}

		private async Task Tick()
		{
			await Task.FromResult(0);
		}
	}
}
