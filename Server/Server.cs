using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using IgiCore.Core;
using IgiCore.Core.Models.Economy.Banking;
using IgiCore.Core.Models.Objects.Vehicles;
using IgiCore.Core.Rpc;
using IgiCore.Core.Services;
using IgiCore.Server.Controllers;
using IgiCore.Server.Managers;
using IgiCore.Server.Models.Economy.Banking;
using IgiCore.Server.Models.Player;
using IgiCore.Server.Rpc;
using IgiCore.Server.Services;
using IgiCore.Server.Services.Economy;
using IgiCore.Server.Storage.MySql;
using JetBrains.Annotations;
using Debug = CitizenFX.Core.Debug;

namespace IgiCore.Server
{
	[UsedImplicitly]
	public class Server : BaseScript
	{
		public static Server Instance { get; private set; }
		public static DB Db { get; private set; }

		public readonly ServiceRegistry Services = new ServiceRegistry();

		public new PlayerList Players => base.Players;

		public new EventHandlerDictionary EventHandlers => base.EventHandlers;

		public Server()
		{
			// Singleton
			Instance = this;

			Db = new DB();
			Db.Database.CreateIfNotExists();

			this.Services.Add(new VehicleService());
			this.Services.Add(new BankService());
			this.Services.Add(new CommandService());
			this.Services.Initialize();

			//Client.Event(ServerEvents.ResourceStarting).On<string>(r => Debug.WriteLine($"Starting resource: {r}"));
			//Client.Event(ServerEvents.ResourceStart).On<string>(r => Debug.WriteLine($"Start resource: {r}"));
			//Client.Event(ServerEvents.ResourceStop).On<string>(r => Debug.WriteLine($"Stop resource: {r}"));

			Client.Event(ServerEvents.HostingSession).On(SessionManager.OnHostingSession);
			Client.Event(ServerEvents.HostedSession).On(SessionManager.OnHostedSession);

			Client.Event(ServerEvents.PlayerConnecting).On(PlayerController.Connecting);
			Client.Event(ServerEvents.PlayerDropped).On(PlayerController.Dropped);

			Client.Event(RpcEvents.GetServerInformation).On(ClientController.Ready);
			Client.Event(RpcEvents.ClientDisconnect).On(ClientController.Disconnect);

			Client.Event(RpcEvents.AcceptRules).On(UserController.AcceptRules);
			Client.Event(RpcEvents.GetUser).On(UserController.Load);

			Client.Event(RpcEvents.GetCharacters).On(CharacterController.List);
			Client.Event(RpcEvents.CharacterLoad).On(CharacterController.Load);
			Client.Event(RpcEvents.CharacterCreate).On(CharacterController.Create);
			Client.Event(RpcEvents.CharacterDelete).On(CharacterController.Delete);
			Client.Event(RpcEvents.CharacterSave).On(CharacterController.Save);

			Client.Event(RpcEvents.BankAtmWithdraw).On(BankingController.AtmWithdraw);

			Client.Event(RpcEvents.CarCreate).On(VehicleController.Create<Car>);
			Client.Event(RpcEvents.CarSave).On(VehicleController.Save<Car>);
			Client.Event(RpcEvents.CarTransfer).On(OwnershipController.TransferObject<Car>);
			Client.Event(RpcEvents.CarClaim).On(OwnershipController.ClaimObject<Car>);
			Client.Event(RpcEvents.CarUnclaim).On(OwnershipController.UnclaimObject<Car>);

			Client.Event(RpcEvents.BikeSave).On(VehicleController.Save<Bike>);
			Client.Event(RpcEvents.BikeTransfer).On(OwnershipController.TransferObject<Bike>);
			Client.Event(RpcEvents.BikeClaim).On(OwnershipController.ClaimObject<Bike>);
			Client.Event(RpcEvents.BikeUnclaim).On(OwnershipController.UnclaimObject<Bike>);

			API.SetGameType("Roleplay");
			API.SetMapName("Los Santos");

		    Db.Banks.AddOrUpdate(
		        new Bank
		        {
                    Id = new Guid("39e662f1-4dfe-96b3-ae2f-e36da99c1e80"),
                    Name = "Fleeca",
		            Branches = new List<BankBranch>
		            {
		                new BankBranch
		                {
		                    Name = "Legion Square",
		                    Position = new Vector3(149.7f, -1042.2f, 28.33f),
		                    Heading = 336.00f
		                },
		                new BankBranch
		                {
		                    Name = "Legion Square 02",
		                    Position = new Vector3(163.7f, -1004.2f, 28.35f),
		                    Heading = 280.00f
		                }
		            },
		            Atms = new List<BankAtm>
		            {
		                new BankAtm
		                {
		                    Name = "FLCA LSS 0001",
		                    Hash = 506770882,
		                    Position = new Vector3(147.4731f, -1036.218f, 28.36778f)
		                },
		                new BankAtm
		                {
		                    Name = "FLCA LSS 0002",
		                    Hash = 506770882,
		                    Position = new Vector3(145.8392f, -1035.625f, 28.36778f)
		                },
		                new BankAtm
		                {
		                    Name = "Standup ATM 0001",
		                    Hash = -870868698,
		                    Position = new Vector3(228.0324f, 337.8501f, 104.5013f)
		                },

		            },
		            Accounts = new List<BankAccount>()
		        });
		    Db.SaveChangesAsync();
        }

		[Conditional("DEBUG")]
		public static void Log(string message) => Debug.WriteLine($"{DateTime.Now:s} [SERVER]: {message}");
	}
}
