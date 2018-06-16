using System.Threading.Tasks;
using IgiCore.SDK.Client.Events;
using IgiCore.SDK.Client.Rpc;
using IgiCore.SDK.Core.Diagnostics;
using IgiCore.SDK.Client.Interface;
using IgiCore.SDK.Core.Models.Player;
using JetBrains.Annotations;

namespace TemplatePlugin.Client
{
	[PublicAPI]
	public class Service : IgiCore.SDK.Client.Services.Service
	{

		public Service(ILogger logger, ITickManager ticks, IEventManager events, IRpcHandler rpc, INuiManager nui, User user) : base(logger, ticks, events, rpc, nui, user) { }

		public override async Task Loaded()
		{
			// Runs when the service is initialised.

			await Task.FromResult(0);
		}

		public override async Task Started()
		{
			// Runs when all services have been initialised.

			await Task.FromResult(0);
		}

	}
}
