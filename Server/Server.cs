using System;
using System.Diagnostics;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using IgiCore.Core;
using IgiCore.Core.Models.Objects.Vehicles;
using IgiCore.Core.Rpc;
using IgiCore.Core.Services;
using IgiCore.Server.Handlers;
using IgiCore.Server.Managers;
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

			API.SetGameType("Roleplay");
			API.SetMapName("Los Santos");
		}

		[Conditional("DEBUG")]
		public static void Log(string message) => Debug.WriteLine($"{DateTime.Now:s} [SERVER]: {message}");
	}
}
