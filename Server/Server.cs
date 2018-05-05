﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using IgiCore.Core;
using IgiCore.Core.Extensions;
using IgiCore.Core.Models.Appearance;
using IgiCore.Core.Models.Connection;
using IgiCore.Core.Models.Economy.Banking;
using IgiCore.Core.Models.Objects;
using IgiCore.Core.Services;
using IgiCore.Server.Extentions;
using IgiCore.Server.Models.Economy.Banking;
using IgiCore.Server.Models.Player;
using IgiCore.Server.Rpc;
using IgiCore.Server.Services;
using IgiCore.Server.Services.Economy;
using IgiCore.Server.Storage.MySql;
using Newtonsoft.Json;
using Citizen = CitizenFX.Core.Player;
using Debug = CitizenFX.Core.Debug;

namespace IgiCore.Server
{
	public partial class Server : BaseScript
	{
		public static Server Instance { get; protected set; }
		public static DB Db;
		protected readonly ServiceRegistry Services = new ServiceRegistry();

		public new PlayerList Players => base.Players;

	    public event EventHandler<EventArgs> OnCharacterCreate;

        public Server()
		{
			// Singleton
			Instance = this;

			Db = new DB();
			Db.Database.CreateIfNotExists();

			this.Services.Add(new VehicleService());
            this.Services.Add(new BankService());
			this.Services.Initialise(this.EventHandlers);

			//HandleEvent<string>(ServerEvents.ResourceStarting, r => Debug.WriteLine($"Starting resource: {r}"));
			//HandleEvent<string>(ServerEvents.ResourceStart, r => Debug.WriteLine($"Start resource: {r}"));
			//HandleEvent<string>(ServerEvents.ResourceStop, r => Debug.WriteLine($"Stop resource: {r}"));

			HandleEvent<Citizen>(ServerEvents.HostingSession, SessionManager.OnHostingSession);
			HandleEvent<Citizen>(ServerEvents.HostedSession, SessionManager.OnHostedSession);

			HandleEvent<Citizen, string, CallbackDelegate>(ServerEvents.PlayerConnecting, OnPlayerConnecting);
			HandleEvent<Citizen, string, CallbackDelegate>(ServerEvents.PlayerDropped, OnPlayerDropped);

			HandleEvent<int, string, string>("chatMessage", OnChatMessage);

			HandleEvent<Citizen>(RpcEvents.GetServerInformation, ClientReady);
			HandleEvent<Citizen>(RpcEvents.ClientDisconnect, ClientDisconnect);

			HandleEvent<Citizen, string>(RpcEvents.AcceptRules, AcceptRules);

			HandleEvent<Citizen>(RpcEvents.GetUser, User.Load);
			HandleEvent<Citizen>(RpcEvents.GetCharacters, GetCharacters);

			HandleEvent<Citizen, string>(RpcEvents.CharacterLoad, LoadCharacter);
			HandleEvent<Citizen, string>(RpcEvents.CharacterCreate, CreateCharacter);
			HandleEvent<Citizen, string>(RpcEvents.CharacterDelete, DeleteCharacter);
			HandleJsonEvent<Character>(RpcEvents.CharacterSave, Character.Save);

            HandleEvent<Citizen, string, string, string>(RpcEvents.BankAtmWithdraw, Handlers.Bank.AtmWithdraw);

			//HandleEvent<string>("igi:car:save", VehicleActions.Save<Car>);
			//HandleEvent<string, int>("igi:car:transfer", TransferObject<Car>);
			//HandleEvent<Citizen, string>("igi:car:claim", ClaimObject<Car>);
			//HandleEvent<int>("igi:car:unclaim", UnclaimObject<Car>);

			//HandleEvent<string>("igi:bike:save", VehicleActions.Save<Bike>);
			//HandleEvent<string, int>("igi:bike:transfer", TransferObject<Bike>);
			//HandleEvent<Citizen, string>("igi:bike:claim", ClaimObject<Bike>);
			//HandleEvent<int>("igi:bike:unclaim", UnclaimObject<Bike>);

			API.SetGameType("Roleplay");
			API.SetMapName("Los Santos");


		    Db.Banks.AddOrUpdate(
		        new Bank
		        {
                    Name = "Fleeca",
                    Branches = new List<BankBranch>
                    {
                        new BankBranch
                        {
                            Name = "Legion Square"
                        }
                    },
                    ATMs = new List<BankAtm>
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
                        }
                    },
                    Accounts = new List<BankAccount>()
		        });
		    Db.SaveChangesAsync();
		}

		private static void ClientReady([FromSource] Citizen citizen)
		{
			TriggerClientEvent(citizen, RpcEvents.GetServerInformation, JsonConvert.SerializeObject(new ServerInformation
			{
				ResourceName = API.GetCurrentResourceName(),
				ServerName = Config.ServerName,
				DateTime = DateTime.UtcNow,
				Weather = "EXTRASUNNY", // TODO
                Atms = Db.BankATMs.ToList()
			}));
            
		}

		private static void ClientDisconnect([FromSource] Citizen citizen)
		{
			citizen.Drop("Disconnected");
		}

		private static async void AcceptRules([FromSource] Citizen citizen, string jsonDateTime)
		{
			var user = await User.GetOrCreate(citizen);

			user.AcceptedRules = JsonConvert.DeserializeObject<DateTime>(jsonDateTime);

			Db.Users.AddOrUpdate(user);
			await Db.SaveChangesAsync();
		}

		private static async void GetCharacters([FromSource] Citizen citizen)
		{
			User user = await User.GetOrCreate(citizen);

			if (user.Characters == null) user.Characters = new List<Character>();

			TriggerClientEvent(citizen, RpcEvents.GetCharacters, JsonConvert.SerializeObject(user.Characters.NotDeleted().OrderBy(c => c.Created)));
		}

		private static async void CreateCharacter([FromSource] Citizen citizen, string characterJson)
		{
			User user = await User.GetOrCreate(citizen);

			if (user.Characters == null) user.Characters = new List<Character>();

			Character character = JsonConvert.DeserializeObject<Character>(characterJson);
			character.Id = GuidGenerator.GenerateTimeBasedGuid();
			character.Alive = true;
			character.Health = 10000;
			character.Armor = 0;
			character.Ssn = "123-45-6789";
			//character.Position = new Vector3 { X = -1038.121f, Y = -2738.279f, Z = 20.16929f };
			character.Position = new Vector3 { X = 153.7846f, Y = -1032.899f, Z = 29.33798f };
			character.LastPlayed = DateTime.MinValue;
			character.Created = DateTime.UtcNow;
			character.Style = new Style { Id = GuidGenerator.GenerateTimeBasedGuid() };
            //character.Inventory = new Inventory { Id = GuidGenerator.GenerateTimeBasedGuid() };

		    foreach (ServerService service in Server.Instance.Services) character = await service.OnCharacterCreate(character);

            user.Characters.Add(character);

			Db.Users.AddOrUpdate(user);
			await Db.SaveChangesAsync();

			GetCharacters(citizen);
		}

		private static async void DeleteCharacter([FromSource] Citizen citizen, string characterId)
		{
			User user = await User.GetOrCreate(citizen);

			if (user.Characters == null) user.Characters = new List<Character>();

			var id = Guid.Parse(characterId);

			var character = user.Characters.FirstOrDefault(c => c.Id == id);

			if (character == null) return;

			character.Deleted = DateTime.UtcNow;

			Db.Characters.AddOrUpdate(character);
			await Db.SaveChangesAsync();

			GetCharacters(citizen);
		}

		private static async void LoadCharacter([FromSource] Citizen citizen, string characterId)
		{
			User user = await User.GetOrCreate(citizen);

			if (user.Characters == null) user.Characters = new List<Character>();

			var id = Guid.Parse(characterId);

			var character = user.Characters.NotDeleted().FirstOrDefault(c => c.Id == id);

			if (character == null) return;

			character.LastPlayed = DateTime.UtcNow;

			Db.Characters.AddOrUpdate(character);
			await Db.SaveChangesAsync();

			TriggerClientEvent(citizen, RpcEvents.CharacterLoad, JsonConvert.SerializeObject(character));
		}

		private void TransferObject<T>(string objJson, int playerId) where T : class, IObject
		{
			T obj = JsonConvert.DeserializeObject<T>(objJson);
			obj.Id = Db.Set<T>().First(c => c.Handle == obj.Handle).Id;

			TriggerClientEvent(this.Players[playerId], $"igi:{typeof(T).Name}:claim", JsonConvert.SerializeObject(obj));
		}

		private async Task ClaimObject<T>([FromSource] Citizen claimer, string objectIdString) where T : class, IObject
		{
			Log($"{objectIdString}");

			var claimerSteamId = claimer.Identifiers["steam"];
			User claimerUser = Db.Users.First(u => u.SteamId == claimerSteamId);

			Guid objectId = Guid.Parse(objectIdString);
			T obj = Db.Set<T>().First(c => c.Id == objectId);

			Guid currentTrackerId = obj.TrackingUserId;
			obj.TrackingUserId = claimerUser.Id;

			Db.Set<T>().AddOrUpdate(obj);
			await Db.SaveChangesAsync();

			if (currentTrackerId == Guid.Empty) return;

			User currentTrackerUser = Db.Users.First(u => u.Id == currentTrackerId);
			try
			{
				TriggerClientEvent(
					this.Players.First(p => p.Identifiers["steam"] == currentTrackerUser.SteamId),
					$"igi:{typeof(T).Name}:unclaim",
					JsonConvert.SerializeObject(obj));
			}
			catch (Exception ex) { Log(ex.Message); }
		}

		private static async Task UnclaimObject<T>(int netId) where T : class, IObject
		{
			T obj = Db.Set<T>().First(c => c.NetId == netId);
			obj.TrackingUserId = Guid.Empty;
			obj.Handle = null;
			obj.NetId = null;

			Db.Set<T>().AddOrUpdate(obj);
			await Db.SaveChangesAsync();
		}

		[Conditional("DEBUG")] public static void Log(string message) { Debug.WriteLine($"{DateTime.Now:s} [SERVER]: {message}"); }

		public void HandleEvent(string name, Action action) { this.EventHandlers[name] += action; }

		public void HandleEvent<T1>(string name, Action<T1> action) { this.EventHandlers[name] += action; }

		public void HandleEvent<T1, T2>(string name, Action<T1, T2> action) { this.EventHandlers[name] += action; }

		public void HandleEvent<T1, T2, T3>(string name, Action<T1, T2, T3> action) { this.EventHandlers[name] += action; }

	    public void HandleEvent<T1, T2, T3, T4>(string name, Action<T1, T2, T3, T4> action) { this.EventHandlers[name] += action; }

        public void HandleJsonEvent<T>(string eventName, Action<T> action) { this.EventHandlers[eventName] += new Action<string>(json => action(JsonConvert.DeserializeObject<T>(json))); }

		public void HandleJsonEvent<T1, T2>(string eventName, Action<T1, T2> action) { this.EventHandlers[eventName] += new Action<string, string>((j1, j2) => action(JsonConvert.DeserializeObject<T1>(j1), JsonConvert.DeserializeObject<T2>(j2))); }

		public void HandleJsonEvent<T1, T2, T3>(string eventName, Action<T1, T2, T3> action) { this.EventHandlers[eventName] += new Action<string, string, string>((j1, j2, j3) => action(JsonConvert.DeserializeObject<T1>(j1), JsonConvert.DeserializeObject<T2>(j2), JsonConvert.DeserializeObject<T3>(j3))); }
	}
}
