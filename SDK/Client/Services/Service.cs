using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using IgiCore.SDK.Client.Events;
using IgiCore.SDK.Client.Rpc;
using IgiCore.SDK.Core.Diagnostics;
using JetBrains.Annotations;

namespace IgiCore.SDK.Client.Services
{
	[UsedImplicitly]
	public abstract class Service
	{
		protected readonly ILogger Logger;
		protected readonly ITickManager Ticks;
		protected readonly IEventManager Events;
		protected readonly IRpcHandler Rpc;

		protected Service(ILogger logger, ITickManager ticks, IEventManager events, IRpcHandler rpc)
		{
			this.Logger = logger;
			this.Ticks = ticks;
			this.Events = events;
			this.Rpc = rpc;
		}

		protected async Task Delay(int msec)
		{
			await BaseScript.Delay(msec);
		}

		protected async Task Delay(TimeSpan delay)
		{
			await BaseScript.Delay((int)delay.TotalMilliseconds);
		}
	}
}
