using System.Threading.Tasks;
using IgiCore.SDK.Client.Rpc;
using IgiCore.SDK.Core.Diagnostics;
using JetBrains.Annotations;

namespace IgiCore.SDK.Client
{
	[UsedImplicitly]
	public abstract class Service
	{
		protected readonly ILogger Logger;
		protected readonly IEventsManager Events;

		public Service(ILogger logger, IEventsManager events)
		{
			this.Logger = logger;
			this.Events = events;
		}

		public abstract Task Tick();
	}
}
