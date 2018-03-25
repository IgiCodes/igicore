using CitizenFX.Core;
using IgiCore.Core.Extensions;
using IgiCore.Core.Models.Objects;
using IgiCore.Core.Models.Objects.Vehicles;
using IgiCore.Core.Services;
using IgiCore.Server.Models;
using IgiCore.Server.Services;
using IgiCore.Server.Storage.MySql;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using IgiCore.Server.Models.Objects.Vehicles;
using Citizen = CitizenFX.Core.Player;

namespace IgiCore.Server
{
	public partial class Server : BaseScript
	{
		protected ServiceRegistry Services = new ServiceRegistry();

		public static DB Db;

		public new PlayerList Players => base.Players;

		public Server()
		{
			Db = new DB();
			Db.Database.CreateIfNotExists();

			this.Services.Add(new VehicleService(this));
			this.Services.Initialise(this.EventHandlers);

			//HandleEvent<string>("onResourceStarting", r => Debug.WriteLine($"Starting resource: {r}"));
			//HandleEvent<string>("onResourceStart", r => Debug.WriteLine($"Start resource: {r}"));
			//HandleEvent<string>("onResourceStop", r => Debug.WriteLine($"Stop resource: {r}"));

			HandleEvent<Citizen, string, CallbackDelegate>("playerConnecting", OnPlayerConnecting);
			HandleEvent<Citizen, string, CallbackDelegate>("playerDropped", OnPlayerDropped);

			HandleEvent<int, string, string>("chatMessage", OnChatMessage);

			HandleEvent<Citizen>("igi:user:load", User.Load);
			HandleJsonEvent<Character>("igi:character:save", Character.Save);

			HandleEvent<string>("igi:car:save", VehicleActions.Save<Car>);
			HandleEvent<string, int>("igi:car:transfer", TransferObject<Car>);
			HandleEvent<Citizen, string>("igi:car:claim", ClaimObject<Car>);
			HandleEvent<int>("igi:car:unclaim", UnclaimObject<Car>);

			HandleEvent<string>("igi:bike:save", VehicleActions.Save<Bike>);
			HandleEvent<string, int>("igi:bike:transfer", TransferObject<Bike>);
			HandleEvent<Citizen, string>("igi:bike:claim", ClaimObject<Bike>);
			HandleEvent<int>("igi:bike:unclaim", UnclaimObject<Bike>);
		}

		private void TransferObject<T>(string objJson, int playerId) where T : class, IObject
		{
			T obj = JsonConvert.DeserializeObject<T>(objJson);
			obj.Id = Db.Set<T>().First(c => c.Handle == obj.Handle).Id;

			TriggerClientEvent(this.Players[playerId], $"igi:{typeof(T).Name}:claim", JsonConvert.SerializeObject(obj));
		}

		private void ClaimObject<T>([FromSource] Citizen claimer, string objectIdString) where T : class, IObject
		{
			Log($"{objectIdString}");

			string claimerSteamId = claimer.Identifiers["steam"];
			User claimerUser = Db.Users.First(u => u.SteamId == claimerSteamId);

			Guid objectId = Guid.Parse(objectIdString);
			T obj = Db.Set<T>().First(c => c.Id == objectId);

			Guid currentTrackerId = obj.TrackingUserId;
			obj.TrackingUserId = claimerUser.Id;

			Db.Set<T>().AddOrUpdate(obj);
			Db.SaveChanges();

			if (currentTrackerId == Guid.Empty) return;

			User currentTrackerUser = Db.Users.First(u => u.Id == currentTrackerId);
			try
			{
				TriggerClientEvent(this.Players.First(p => p.Identifiers["steam"] == currentTrackerUser.SteamId), $"igi:{typeof(T).Name}:unclaim", JsonConvert.SerializeObject(obj));
			}
			catch (Exception ex)
			{
				Log(ex.Message);
			}
		}

		private static void UnclaimObject<T>(int netId) where T : class, IObject
		{
			T obj = Db.Set<T>().First(c => c.NetId == netId);
			obj.TrackingUserId = Guid.Empty;
			obj.Handle = null;
			obj.NetId = null;

			Db.Set<T>().AddOrUpdate(obj);
			Db.SaveChanges();
		}

		private static Character NewCharCommand(Citizen citizen, string charName)
		{
			User user = User.GetOrCreate(citizen);
			if (user.Characters == null) user.Characters = new List<Character>();

			Character character = new Character
			{
				Name = charName,
				Style = new Core.Models.Appearance.Style { Id = GuidGenerator.GenerateTimeBasedGuid() }
			};

			user.Characters.Add(character);

			Db.Users.AddOrUpdate(user);
			Db.SaveChanges();

			return character;
		}

		private static Character GetCharCommand(Citizen citizen, Guid charId) => User.GetOrCreate(citizen).Characters.FirstOrDefault(c => c.Id == charId);
		private static Character GetCharCommand(Citizen citizen, string name) => User.GetOrCreate(citizen).Characters.FirstOrDefault(c => c.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

		[System.Diagnostics.Conditional("DEBUG")]
		public static void Log(string message)
		{
			Debug.WriteLine($"{DateTime.Now:s} [SERVER]: {message}");
		}

		public void HandleEvent(string name, Action action) => this.EventHandlers[name] += action;
		public void HandleEvent<T1>(string name, Action<T1> action) => this.EventHandlers[name] += action;
		public void HandleEvent<T1, T2>(string name, Action<T1, T2> action) => this.EventHandlers[name] += action;
		public void HandleEvent<T1, T2, T3>(string name, Action<T1, T2, T3> action) => this.EventHandlers[name] += action;
		public void HandleJsonEvent<T>(string eventName, Action<T> action) => this.EventHandlers[eventName] += new Action<string>(json => action(JsonConvert.DeserializeObject<T>(json)));
		public void HandleJsonEvent<T1, T2>(string eventName, Action<T1, T2> action) => this.EventHandlers[eventName] += new Action<string, string>((j1, j2) => action(JsonConvert.DeserializeObject<T1>(j1), JsonConvert.DeserializeObject<T2>(j2)));
		public void HandleJsonEvent<T1, T2, T3>(string eventName, Action<T1, T2, T3> action) => this.EventHandlers[eventName] += new Action<string, string, string>((j1, j2, j3) => action(JsonConvert.DeserializeObject<T1>(j1), JsonConvert.DeserializeObject<T2>(j2), JsonConvert.DeserializeObject<T3>(j3)));
	}
}
