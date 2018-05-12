using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using IgiCore.Core.Models.Economy.Banking;
using IgiCore.Core.Models.Objects.Vehicles;
using IgiCore.Core.Rpc;
using IgiCore.Core.Services;
using IgiCore.Server.Controllers;
using IgiCore.Server.Managers;
using IgiCore.Server.Models.Economy.Banking;
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

			API.SetGameType(ConfigurationManager.Configuration.GameType);
			API.SetMapName(ConfigurationManager.Configuration.MapName);

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

			Client.Event(ServerEvents.PlayerConnecting).On(PlayerActions.Connecting);
			Client.Event(ServerEvents.PlayerDropped).On(PlayerActions.Dropped);

			Client.Event(RpcEvents.GetServerInformation).On(ClientActions.Ready);
			Client.Event(RpcEvents.ClientDisconnect).On(ClientActions.Disconnect);

			Client.Event(RpcEvents.AcceptRules).On(UserActions.AcceptRules);
			Client.Event(RpcEvents.GetUser).On(UserActions.Load);

			Client.Event(RpcEvents.GetCharacters).On(CharacterActions.List);
			Client.Event(RpcEvents.CharacterLoad).On(CharacterActions.Load);
			Client.Event(RpcEvents.CharacterCreate).On(CharacterActions.Create);
			Client.Event(RpcEvents.CharacterDelete).On(CharacterActions.Delete);
			Client.Event(RpcEvents.CharacterSave).On(CharacterActions.Save);

			Client.Event(RpcEvents.BankAtmWithdraw).On(BankingActions.AtmWithdraw);

			Client.Event(RpcEvents.CarSave).On(VehicleActions.Save<Car>);
			Client.Event(RpcEvents.CarTransfer).On(OwnershipActions.TransferObject<Car>);
			Client.Event(RpcEvents.CarClaim).On(OwnershipActions.ClaimObject<Car>);
			Client.Event(RpcEvents.CarUnclaim).On(OwnershipActions.UnclaimObject<Car>);

			Client.Event(RpcEvents.BikeSave).On(VehicleActions.Save<Bike>);
			Client.Event(RpcEvents.BikeTransfer).On(OwnershipActions.TransferObject<Bike>);
			Client.Event(RpcEvents.BikeClaim).On(OwnershipActions.ClaimObject<Bike>);
			Client.Event(RpcEvents.BikeUnclaim).On(OwnershipActions.UnclaimObject<Bike>);
			
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
