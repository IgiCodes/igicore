using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using IgiCore.SDK.Server.Configuration;
using IgiCore.SDK.Server.Controllers;
using IgiCore.Server.Configuration;
using IgiCore.Server.Controllers;
using IgiCore.Server.Diagnostics;
using IgiCore.Server.Events;
using IgiCore.Server.Plugins;
using IgiCore.Server.Rpc;
using JetBrains.Annotations;

namespace IgiCore.Server
{
	[UsedImplicitly]
	public class Program : BaseScript
	{
		private readonly List<Controller> controllers = new List<Controller>();

		public Program()
		{
			Logger logger = new Logger();

			// Set the AppDomain working directory to the current resource root
			Environment.CurrentDirectory = FileManager.ResolveResourcePath();

			var config = ConfigurationManager.Load<CoreConfiguration>("igicore");

			ServerConfiguration.LogLevel = config.Log.Level;
			API.SetMapName(config.Display.Map);
			API.SetGameType(config.Display.Map);
			
			// Setup RPC handlers
			RpcManager.Configure(this.EventHandlers);

			var events = new EventManager();

			// Load core controllers
			this.controllers.Add(new DatabaseController(new Logger("Database"), events, new RpcHandler(), ConfigurationManager.Load<DatabaseConfiguration>("database")));
			this.controllers.Add(new SessionController(new Logger("Session"), events, new RpcHandler()));
			this.controllers.Add(new ClientController(new Logger("Client"), events, new RpcHandler()));

			// Parse the master plugin definition file
			ServerPluginDefinition definition = PluginManager.LoadDefinition();

			// Resolve dependencies
			PluginDefinitionGraph dependencyGraph = definition.ResolveDependencies();

			// Load plugins into the AppDomain
			foreach (ServerPluginDefinition plugin in dependencyGraph.Definitions)
			{
				// Load include files
				foreach (string includeName in plugin.Definition.Server.Include)
				{
					string includeFile = Path.Combine(plugin.Location, $"{includeName}.net.dll");
					if (!File.Exists(includeFile)) throw new FileNotFoundException(includeFile);

					AppDomain.CurrentDomain.Load(File.ReadAllBytes(includeFile));
				}

				// Load main files
				foreach (string mainName in plugin.Definition.Server.Main)
				{
					string mainFile = Path.Combine(plugin.Location, $"{mainName}.net.dll");
					if (!File.Exists(mainFile)) throw new FileNotFoundException(mainFile);

					// Find controllers
					foreach (Type controllerType in Assembly.LoadFrom(mainFile).GetTypes().Where(t => !t.IsAbstract && (t.IsSubclassOf(typeof(Controller)) || t.IsSubclassOf(typeof(ConfigurableController<>)))))
					{
						List<object> constructorArgs = new List<object>
						{
							new Logger($"Plugin|{plugin.Definition.Name}"),
							events,
							new RpcHandler()
						};

						// Check if controller is configurable
						if (controllerType.BaseType != null && controllerType.BaseType.IsGenericType && controllerType.BaseType.GetGenericTypeDefinition() == typeof(ConfigurableController<>))
						{
							// Get controller configuration type
							Type configurationType = controllerType.BaseType.GetGenericArguments()[0];

							string configFile = Path.Combine(PluginManager.ConfigurationPath, $"{plugin.Definition.Name}.yml");
							if (!File.Exists(configFile)) throw new FileNotFoundException("Unable to find plugin configuration file", configFile);

							// Load configuration
							object configuration = ConfigurationManager.Load(plugin.Definition.Name, configurationType);

							constructorArgs.Add(configuration);
						}

						// Construct controller instance
						Controller controller = (Controller)Activator.CreateInstance(controllerType, constructorArgs.ToArray());

						this.controllers.Add(controller);
					}
				}
			}

			logger.Info($"Plugins loaded, {this.controllers.Count} controller(s) created");
		}
	}
}
