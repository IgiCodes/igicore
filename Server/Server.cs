using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using CitizenFX.Core;
using IgiCore.Core.Services;
using IgiCore.SDK.Core.Rpc;
using IgiCore.SDK.Server;
using IgiCore.SDK.Server.Configuration;
using IgiCore.Server.Controllers;
using IgiCore.Server.Diagnostics;
using IgiCore.Server.Managers;
using IgiCore.Server.Plugins;
using IgiCore.Server.Rpc;
using IgiCore.Server.Storage.MySql;
using JetBrains.Annotations;
using MySql.Data.EntityFramework;
using Newtonsoft.Json;
using SemVer;
using SimpleInjector;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using Client = IgiCore.Server.Rpc.Client;
using Version = SemVer.Version;

namespace IgiCore.Server
{
	class ServerConfiguration : IConfiguration
	{
		public string DatabaseConnection { get; set; }
	}

	[UsedImplicitly]
	public class Server : BaseScript
	{
		public static Server Instance { get; private set; }

		public static DB Db { get; private set; }

		static Container container;

		public readonly List<ServerController> Controllers = new List<ServerController>();
		public readonly ServiceRegistry Services = new ServiceRegistry();

		public new PlayerList Players => base.Players;

		public new EventHandlerDictionary EventHandlers => base.EventHandlers;

		public Server()
		{
			// Singleton
			Instance = this;
			
			//MySqlTrace.Switch.Level = SourceLevels.All;
			//MySqlTrace.Listeners.Add(new ConsoleTraceListener());

			//DbConfiguration.Loaded += (_, d) => d.AddDefaultResolver(new MySqlDependencyResolver());
			DbConfiguration.SetConfiguration(new MySqlEFConfiguration());

			using (var entities = new DB())
			{
				if (!entities.Database.Exists())
				{
					Log("Creating new database");

					entities.Database.CreateIfNotExists();
				}
			}


			var serverEventsManager = new ServerEventsManager();
			var clientEventsManager = new ClientEventsManager();
			var serverConfiguration = new ServerConfiguration
			{
				DatabaseConnection = "Host=harvest;Port=3306;Database=fivem;User Id=root;Password=password;CharSet=utf8mb4;SSL Mode=None"
			};



			Environment.CurrentDirectory = Path.GetDirectoryName(ConfigurationManager.ResolveCurrentPath("igicore.yml") ?? AppDomain.CurrentDomain.BaseDirectory);
		
			var pluginPath = Path.Combine(Environment.CurrentDirectory ?? ".", "Plugins");

			if (!Directory.Exists(pluginPath)) throw new DirectoryNotFoundException($"Unable to find plugins directory at {pluginPath}");

			Deserializer deserializer = new DeserializerBuilder()
				.WithNamingConvention(new CamelCaseNamingConvention())
				//.IgnoreUnmatchedProperties()
				.Build();

			var pluginDefinitions = new List<ServerPluginDefinition>();

			foreach (var directory in Directory.EnumerateDirectories(pluginPath, "*", SearchOption.TopDirectoryOnly))
			{
				var definitionFile = Path.Combine(directory, "plugin.yml");
				if (!File.Exists(definitionFile)) throw new FileNotFoundException($"Unable to find plugin definition at {definitionFile}");
				
				var definition = deserializer.Deserialize<PluginDefinition>(File.ReadAllText(definitionFile));
				
				var plugin = new ServerPluginDefinition
				{
					Path = directory,
					Definition = definition
				};

				pluginDefinitions.Add(plugin);
			}
			
			foreach (var plugin in pluginDefinitions)
			{
				foreach (var dependency in plugin.Definition.Server.Dependencies)
				{
					var node = pluginDefinitions.FirstOrDefault(p => p.Definition.Name == dependency.Key);
					if (node == null) throw new Exception($"Unable to find dependency {dependency} required by {plugin.Definition.Name}");

					var version = new Version(node.Definition.Version);
					var required = new Range(dependency.Value);

					if (!required.IsSatisfied(version)) throw new VersionNotFoundException($"{plugin.Definition.Name}@{plugin.Definition.Version} requires {node.Definition.Name}@{dependency.Value} but {node.Definition.Name}@{node.Definition.Version} was found");

					plugin.Definition.Server.DependencyNodes.Add(node);
				}
			}

			//Log($"{JsonConvert.SerializeObject(pluginDefinitions, Formatting.Indented)}");

			pluginDefinitions = Graph.TopologicalSort(pluginDefinitions);
			
			Log("Loading plugins:");

			foreach (var plugin in pluginDefinitions)
			{
				Log($"Loading {plugin.Definition.Name} binaries:");

				foreach (var includeName in plugin.Definition.Server.Include)
				{
					var includeFile = Path.Combine(plugin.Path, $"{includeName}.net.dll");

					if (!File.Exists(includeFile)) throw new FileNotFoundException(includeFile);

					Log($"  Include: {includeFile.Substring(Environment.CurrentDirectory.Length)}");
					AppDomain.CurrentDomain.Load(File.ReadAllBytes(includeFile));
				}

				foreach (var mainName in plugin.Definition.Server.Main)
				{
					var mainFile = Path.Combine(plugin.Path, $"{mainName}.net.dll");

					if (!File.Exists(mainFile)) throw new FileNotFoundException(mainFile);

					Log($"  Main: {mainFile.Substring(Environment.CurrentDirectory.Length)}");

					var assembly = Assembly.LoadFrom(mainFile);
					var assemblyTypes = assembly.GetTypes().Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(ServerController)));

					foreach (var type in assemblyTypes)
					{
						Log($"    Controller: {mainFile.Substring(Environment.CurrentDirectory.Length)}");

						this.Controllers.Add((ServerController)Activator.CreateInstance(type, new Logger(), serverEventsManager, clientEventsManager, serverConfiguration));
					}
				}
			}

			Log($"Finished loading plugins, {this.Controllers.Count} controllers created");

			return;



			Db = new DB();

			//// Setup IoC container
			//container = new Container();
			//container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

			//// Register DI types
			//container.Register<ILogger, Logger>();

			//container.Verify();



			////this.Controllers.Add(container.GetInstance<ResourceController>());


			//// TODO: Find and load types from plugin DLL files with reflection
			//var serviceTypes = new List<Type>
			//{
			//	typeof(VehicleService),
			//	//typeof(BankService),
			//	typeof(CommandService)
			//};

			//// TODO: From YML
			//var enabledServices = new List<string>
			//{
			//	"VehicleService",
			//	"BankService",
			//	//"CommandService"
			//};

			//foreach (var service in serviceTypes.Where(t => enabledServices.Contains(t.Name)))
			//{
			//	this.Services.Add((Service)container.GetInstance(service));
			//}

			////this.Services.Add(new VehicleService());
			////this.Services.Add(new BankService());
			////this.Services.Add(new CommandService());
			//this.Services.Initialize();


			Client.Event(ServerEvents.HostingSession).On(SessionManager.OnHostingSession);
			Client.Event(ServerEvents.HostedSession).On(SessionManager.OnHostedSession);

			Client.Event(ServerEvents.PlayerConnecting).On(PlayerController.Connecting);
			Client.Event(ServerEvents.PlayerDropped).On(PlayerController.Dropped);

			Client.Event(RpcEvents.GetServerInformation).On(ClientController.Ready);
			Client.Event(RpcEvents.ClientDisconnect).On(ClientController.Disconnect);

			//Client.Event(RpcEvents.AcceptRules).On(UserController.AcceptRules);
			//Client.Event(RpcEvents.GetUser).On(UserController.Load);

			//Client.Event(RpcEvents.GetCharacters).On(CharacterController.List);
			//Client.Event(RpcEvents.CharacterLoad).On(CharacterController.Load);
			//Client.Event(RpcEvents.CharacterCreate).On(CharacterController.Create);
			//Client.Event(RpcEvents.CharacterDelete).On(CharacterController.Delete);
			//Client.Event(RpcEvents.CharacterSave).On(CharacterController.Save);

			//Client.Event(RpcEvents.BankAtmWithdraw).On(BankingController.AtmWithdraw);

			//Client.Event(RpcEvents.CarCreate).On(VehicleController.Create<Car>);
			//Client.Event(RpcEvents.CarSave).On(VehicleController.Save<Car>);
			//Client.Event(RpcEvents.CarTransfer).On(OwnershipController.TransferObject<Car>);
			//Client.Event(RpcEvents.CarClaim).On(OwnershipController.ClaimObject<Car>);
			//Client.Event(RpcEvents.CarUnclaim).On(OwnershipController.UnclaimObject<Car>);

			//Client.Event(RpcEvents.BikeSave).On(VehicleController.Save<Bike>);
			//Client.Event(RpcEvents.BikeTransfer).On(OwnershipController.TransferObject<Bike>);
			//Client.Event(RpcEvents.BikeClaim).On(OwnershipController.ClaimObject<Bike>);
			//Client.Event(RpcEvents.BikeUnclaim).On(OwnershipController.UnclaimObject<Bike>);

			//API.SetGameType("Roleplay");
			//API.SetMapName("Los Santos");

			//Db.Banks.AddOrUpdate(
			//    new Bank
			//    {
			//              Id = new Guid("39e662f1-4dfe-96b3-ae2f-e36da99c1e80"),
			//              Name = "Fleeca",
			//        Branches = new List<BankBranch>
			//        {
			//            new BankBranch
			//            {
			//                Name = "Legion Square",
			//                Position = new Vector3(149.7f, -1042.2f, 28.33f),
			//                Heading = 336.00f
			//            },
			//            new BankBranch
			//            {
			//                Name = "Legion Square 02",
			//                Position = new Vector3(163.7f, -1004.2f, 28.35f),
			//                Heading = 280.00f
			//            }
			//        },
			//        Atms = new List<BankAtm>
			//        {
			//            new BankAtm
			//            {
			//                Name = "FLCA LSS 0001",
			//                Hash = 506770882,
			//                Position = new Vector3(147.4731f, -1036.218f, 28.36778f)
			//            },
			//            new BankAtm
			//            {
			//                Name = "FLCA LSS 0002",
			//                Hash = 506770882,
			//                Position = new Vector3(145.8392f, -1035.625f, 28.36778f)
			//            },
			//            new BankAtm
			//            {
			//                Name = "Standup ATM 0001",
			//                Hash = -870868698,
			//                Position = new Vector3(228.0324f, 337.8501f, 104.5013f)
			//            },

			//        },
			//        Accounts = new List<BankAccount>()
			//    });
			//Db.SaveChanges();
		}

		[Conditional("DEBUG")]
		public static void Log(string message) => Console.WriteLine($"{DateTime.Now:s} {message}");
	}
}
