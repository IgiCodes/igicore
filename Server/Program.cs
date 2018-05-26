using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CitizenFX.Core;
using IgiCore.SDK.Core.Rpc;
using IgiCore.SDK.Server;
using IgiCore.SDK.Server.Configuration;
using IgiCore.Server.Configuration;
using IgiCore.Server.Controllers;
using IgiCore.Server.Diagnostics;
using IgiCore.Server.Events;
using IgiCore.Server.Plugins;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace IgiCore.Server
{
	public class Program : BaseScript
	{
		private readonly Logger logger = new Logger();
		private readonly List<Controller> controllers = new List<Controller>();
		
		public Program()
		{
			// Set the AppDomain working directory to the current resource root
			Environment.CurrentDirectory = FileManager.ResolveResourcePath();

			var eventsManager = new EventsManager(new Logger("Events"), this.EventHandlers);



			Deserializer deserializer2 = new DeserializerBuilder()
				.WithNamingConvention(new CamelCaseNamingConvention())
				//.IgnoreUnmatchedProperties()
				.Build();

			var databaseController = new DatabaseController(new Logger("Database"), eventsManager, deserializer2.Deserialize<DatabaseConfiguration>(File.ReadAllText("config/database.yml")));
			//LoadConfig(databaseController, "database");


			ServerConfiguration.DatabaseConnection = databaseController.Configuration.ToString();
			this.logger.Log(ServerConfiguration.DatabaseConnection);


			//this.controllers.Add(databaseController);

			//this.controllers.Add(new SessionController(new Logger("Session"), eventsManager));
			//this.controllers.Add(new ClientController(new Logger("Client"), eventsManager));


			//Client.Event(RpcEvents.GetServerInformation).On(ClientController.Ready);
			//Client.Event(RpcEvents.ClientDisconnect).On(ClientController.Disconnect);


			// Parse the master plugin definition file
			ServerPluginDefinition definition = PluginManager.LoadDefinition();

			// Resolve dependencies
			PluginDefinitionGraph dependencyGraph = definition.ResolveDependencies();
			//Log($"{JsonConvert.SerializeObject(dependencyGraph, Formatting.Indented)}");

			this.logger.Log($"----------------------------");

			// Load plugins into the AppDomain
			foreach (ServerPluginDefinition plugin in dependencyGraph.Definitions)
			{
				// Load include files
				foreach (var includeName in plugin.Definition.Server.Include)
				{
					var includeFile = Path.Combine(plugin.Location, $"{includeName}.net.dll");
					if (!File.Exists(includeFile)) throw new FileNotFoundException(includeFile);

					AppDomain.CurrentDomain.Load(File.ReadAllBytes(includeFile));
				}

				// Load main files
				foreach (var mainName in plugin.Definition.Server.Main)
				{
					var mainFile = Path.Combine(plugin.Location, $"{mainName}.net.dll");
					if (!File.Exists(mainFile)) throw new FileNotFoundException(mainFile);
					
					// Find controllers
					foreach (Type controllerType in Assembly.LoadFrom(mainFile).GetTypes().Where(t => !t.IsAbstract && (t.IsSubclassOf(typeof(Controller)) || t.IsSubclassOf(typeof(ConfigurableController<>)))))
					{
						var constructorArgs = new List<object>()
						{
							new Logger(),
							eventsManager
						};

						if (controllerType.BaseType != null && controllerType.BaseType.IsGenericType && controllerType.BaseType.GetGenericTypeDefinition() == typeof(ConfigurableController<>))
						{
							var configurationType = controllerType.BaseType.GetGenericArguments()[0];

							var configFile = Path.Combine("config", $"{plugin.Definition.Name}.yml");
							if (!File.Exists(configFile)) return;

							Deserializer deserializer = new DeserializerBuilder()
								.WithNamingConvention(new CamelCaseNamingConvention())
								//.IgnoreUnmatchedProperties()
								.Build();

							object config = deserializer.Deserialize(File.ReadAllText(configFile), configurationType);

							constructorArgs.Add(config);
						}

						var controller = (Controller)Activator.CreateInstance(controllerType, constructorArgs.ToArray());

						this.controllers.Add(controller);
					}
				}
			}

			this.logger.Log($"----------------------------");


			this.logger.Log($"Plugins loaded, {this.controllers.Count} controller(s) created");
		}

		private void LoadConfig(Controller controller, string name)
		{
			// Check if controller is configurable
			var configurationInterface = controller.GetType().GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IConfigurableController<>));
			if (configurationInterface == null) return;

			// Get configuration type
			var configurationType = configurationInterface.GetGenericArguments()[0];
			
			var configFile = Path.Combine("config", $"{name}.yml");
			if (!File.Exists(configFile)) return;

			Deserializer deserializer = new DeserializerBuilder()
				.WithNamingConvention(new CamelCaseNamingConvention())
				//.IgnoreUnmatchedProperties()
				.Build();

			dynamic config = deserializer.Deserialize(File.ReadAllText(configFile), configurationType);

			((dynamic)controller).Configuration = config;
		}
	}
}
