using System;
using System.Linq;
using System.Reflection;
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
		private readonly Logger logger = new Logger();

		/// <summary>
		/// Primary client entrypoint.
		/// Initializes a new instance of the <see cref="Program"/> class.
		/// </summary>
		public Program()
		{
			Startup();
		}

		private async void Startup()
		{
			// Setup RPC handlers
			RpcManager.Configure(this.EventHandlers);

			var ticks = new TickManager(c => this.Tick += c, c => this.Tick -= c);
			var events = new EventManager();
			var handler = new RpcHandler();

			//new StartupService(new Logger("Startup"), ticks, events, new RpcHandler());

			var user = await handler.Event("ready").Request<User>("1.0.0");
			this.logger.Debug(user.Name);

			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				if (assembly.GetCustomAttribute<ClientPluginAttribute>() == null) continue;

				this.logger.Info(assembly.GetName().Name);

				foreach (var type in assembly.GetTypes().Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(Service))))
				{
					this.logger.Info($"\t{type.FullName}");

					Activator.CreateInstance(type, new Logger($"Plugin|{type.Name}"), ticks, events, handler);
				}
			}

			this.logger.Info("Plugins loaded");
		}
	}
}
