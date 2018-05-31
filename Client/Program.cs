using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CitizenFX.Core;
using IgiCore.Client.Diagnostics;
using IgiCore.Client.Events;
using IgiCore.Client.Rpc;
using IgiCore.Models.Player;
using IgiCore.SDK.Client;
using IgiCore.SDK.Client.Services;
using JetBrains.Annotations;

namespace IgiCore.Client
{
	[UsedImplicitly]
	public class Program : BaseScript
	{
		/// <summary>
		/// Primary client entrypoint.
		/// Initializes a new instance of the <see cref="Program"/> class.
		/// </summary>
		public Program()
		{
			var logger = new Logger();

			// Setup RPC handlers
			RpcManager.Configure(this.EventHandlers);

			var ticks = new TickManager(c => this.Tick += c, c => this.Tick -= c);
			var events = new EventManager();


			//new StartupService(new Logger("Startup"), ticks, events, new RpcHandler());

			Task.Factory.StartNew(async () =>
			{
				logger.Debug("Request");
				var r = await new RpcHandler().Event("ready").Request<User>("1.0.0");
				logger.Debug($"Request: {r.Name}");
			});


			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				if (assembly.GetCustomAttribute<ClientPluginAttribute>() == null) continue;

				logger.Debug(assembly.GetName().Name);

				foreach (var type in assembly.GetTypes().Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(Service))))
				{
					logger.Debug($"\t{type.FullName}");

					Activator.CreateInstance(type, new Logger($"Plugin|{type.Name}"), ticks, events, new RpcHandler());
				}
			}

			logger.Debug("Done");
		}
	}
}
