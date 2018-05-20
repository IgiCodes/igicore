using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using IgiCore.Core;
using IgiCore.Core.Services;
using IgiCore.SDK;
using IgiCore.SDK.Core.Diagnostics;
using IgiCore.SDK.Core.Rpc;
using IgiCore.SDK.Server;
using IgiCore.SDK.Server.Configuration;
using IgiCore.SDK.Server.Rpc;
using IgiCore.SDK.Server.Storage;
using IgiCore.Server.Controllers;
using IgiCore.Server.Diagnostics;
using IgiCore.Server.Managers;
using IgiCore.Server.Migrations;
using IgiCore.Server.Rpc;
using IgiCore.Server.Services;
using IgiCore.Server.Services.Economy;
using IgiCore.Server.Storage.Contexts;
using IgiCore.Server.Storage.MySql;
using JetBrains.Annotations;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using Debug = CitizenFX.Core.Debug;
using MySql.Data.Entity;

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

			DbConfiguration.SetConfiguration(new MySqlEFConfiguration());

			using (var entities = new DB())
			{
				if (!entities.Database.Exists())
				{
					Log("Creating new database");

					entities.Database.CreateIfNotExists();
				}
			}



			container = new Container();
			container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

			// Register DI types
			container.Register<ILogger, Logger>();
			container.RegisterInstance(typeof(IServerEventsManager), new ServerEventsManager());
			container.RegisterInstance(typeof(IClientEventsManager), new ClientEventsManager());
			container.RegisterInstance(typeof(IConfiguration), new ServerConfiguration { DatabaseConnection = Config.MySqlConnString });

			container.Verify();

			var a = typeof(IgiCore.Models.Position);
			var b = typeof(ILogger);

			//var pluginPaths = Directory.EnumerateFiles(@"D:\Desktop\FiveM\server\server-data\resources\igicore\Plugins\Banking\bin\Debug", "Banking.dll", SearchOption.AllDirectories);
			var pluginPaths = new[] { @"Banking.net" };

			//var alreadyLoaded = AppDomain.CurrentDomain.GetAssemblies();
			//foreach (var file in alreadyLoaded)
			//{
			//	Log(file.FullName);
			//}

			//AppDomain.CurrentDomain.AssemblyLoad += (sender, args) =>
			//{
			//	Log($"Loading: {args.LoadedAssembly.FullName}");
			//	Log($"Loading: {args.LoadedAssembly.Modules.First().Name}");


			//};
			//AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
			//{
			//	Log($"Resolving: {args.Name}");
			//	return Assembly.Load(args.Name);
			//};

			//var module = ModuleDefinition.ReadModule(@"resources\igicore\" + pluginPaths[0] + ".dll");
			////Log(module.Name);

			//foreach (var assemblyReference in module.AssemblyReferences)
			//{
			//	Log(assemblyReference.Name);
			//}

			//Log("-");

			//foreach (var moduleReference in module.)
			//{
			//	Log(moduleReference.Name);
			//}


			



			//return;

			foreach (var file in pluginPaths)
			{
				AppDomain.CurrentDomain.Load(File.ReadAllBytes(@"D:\Desktop\FiveM\server\server-data\resources\igicore\Plugins\Banking\Banking.Core.net.dll"));
				AppDomain.CurrentDomain.Load(File.ReadAllBytes(@"D:\Desktop\FiveM\server\server-data\resources\igicore\Plugins\Banking\Banking.Server.net.dll"));

				var assembly = Assembly.LoadFrom(@"D:\Desktop\FiveM\server\server-data\resources\igicore\Plugins\Banking\Banking.Server.net.dll");
				//var assembly = Assembly.Load(file);
				var assemblyTypes = assembly.GetTypes().Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(ServerController))).ToArray();
				//var module = ModuleDefinition.ReadModule(file);

				Log(assemblyTypes.Length.ToString());
				Log(assemblyTypes[0].FullName);

				//Activator.CreateInstance(assemblyTypes[0], null, null, null, null);

				foreach (var type in assemblyTypes)
				{
					this.Controllers.Add((ServerController)Activator.CreateInstance(assemblyTypes[0], new Logger(), new ServerEventsManager(), new ClientEventsManager(), new ServerConfiguration { DatabaseConnection = Config.MySqlConnString }));
					//this.Controllers.Add((ServerController)container.GetInstance(type));
				}
			}

			Log(this.Controllers.Count.ToString());

			//return;

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

			API.SetGameType("Roleplay");
			API.SetMapName("Los Santos");

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
		public static void Log(string message) => Debug.WriteLine($"{DateTime.Now:s} [SERVER]: {message}");
	}
}
