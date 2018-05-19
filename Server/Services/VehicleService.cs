using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using IgiCore.Core;
using IgiCore.Core.Extensions;
using IgiCore.Core.Models.Objects.Vehicles;
using IgiCore.Core.Models.Player;
using IgiCore.Core.Rpc;
using IgiCore.Server.Models.Player;
using IgiCore.Server.Rpc;
using Newtonsoft.Json;

namespace IgiCore.Server.Services
{
	public class VehicleService : ServerService
	{
		private const int VehicleLoadDistance = 500;

		public VehicleService()
		{
			Client.Event(RpcEvents.CharacterSave).On(LoadNearbyVehicles);
			Client.Event(ServerEvents.PlayerDropped).On(ReassignTrackedVehicles);
		}

		public override void Initialize()
		{
			ResetVehicleTracking();
		}

		private static async void ResetVehicleTracking()
		{
			foreach (Vehicle vehicle in Server.Db.Vehicles.ToList())
			{
				vehicle.Handle = null;
				vehicle.NetId = null;
				vehicle.TrackingUserId = Guid.Empty;

				Server.Db.Vehicles.AddOrUpdate(vehicle);
			}

			await Server.Db.SaveChangesAsync();
		}

		private async void ReassignTrackedVehicles([FromSource] Player disconnectedCitizen, string disconnectMessage, CallbackDelegate kickReason)
		{
			Server.Log("ReassignTrackedVehicles called");

			var disconnectedSteamId = disconnectedCitizen.Identifiers["steam"];
			Server.Log($"Disconnected user steam id: {disconnectedSteamId}");

			User disconnectedUser = Server.Db.Users.FirstOrDefault(u => u.SteamId == disconnectedSteamId);
			if (disconnectedUser == null) return;
			Server.Log($"Reassigning tracked vehicles for disconnected player: {disconnectedUser.Name}");

			var vehicles = Server.Db.Vehicles.Where(v => v.TrackingUserId == disconnectedUser.Id).ToList();
			if (!vehicles.Any()) return;

			Server.Log("Vehicles found, looking up online users");

			var users = new List<User>();
			foreach (Player serverPlayer in Server.Instance.Players)
			{
				var steamId = serverPlayer.Identifiers["steam"];
				if (steamId == disconnectedSteamId) continue;

				users.Add(Server.Db.Users.First(u => u.SteamId == steamId));
			}

			Server.Log("Looping through vehicles to assign players");

			var assignedVehicles = new Dictionary<Vehicle, Tuple<User, Character>>();
			foreach (Vehicle vehicle in vehicles)
			{
				foreach (User user in users)
				{
					foreach (Character character in user.Characters)
					{
						if (!(Vector3.Distance(character.Position, vehicle.Position) < VehicleLoadDistance)) continue;

						if (!assignedVehicles.ContainsKey(vehicle))
						{
							assignedVehicles.Add(vehicle, new Tuple<User, Character>(user, character));
						}
						else
						{
							if (Vector3.Distance(character.Position, vehicle.Position) < Vector3.Distance(assignedVehicles[vehicle].Item2.Position, vehicle.Position)) assignedVehicles[vehicle] = new Tuple<User, Character>(user, character);
						}
					}
				}
			}

			if (!assignedVehicles.Any())
			{
				Server.Log("No vehicles assigned to anyone");

				Player hostClient = null;
				try
				{
					hostClient = Server.Instance.Players.First(p => p.Handle == API.GetHostId());
				}
				catch (Exception ex)
				{
					Server.Log(ex.Message);
				}

				foreach (Vehicle vehicle in vehicles)
				{
					if (hostClient != null) BaseScript.TriggerClientEvent(hostClient, "igi:entity:delete", vehicle.NetId, vehicle.Hash);

					vehicle.NetId = null;
					vehicle.Handle = null;
					vehicle.TrackingUserId = Guid.Empty;

					Server.Db.Vehicles.AddOrUpdate(vehicle);
					await Server.Db.SaveChangesAsync();
				}
			}

			foreach (var assignedVehicle in assignedVehicles)
			{
				Player citizen = Server.Instance.Players.First(c => c.Identifiers["steam"] == Server.Db.Users.First(u => u.Id == assignedVehicle.Value.Item1.Id).SteamId);

				AssignVehicle(assignedVehicle.Key, citizen);
			}
		}

		private static void AssignVehicle(IVehicle vehicle, Player citizen)
		{
			Server.Log($"Assinging vehicle to {citizen.Name} via event: 'igi:{vehicle.VehicleType().Name}:claim'");

			BaseScript.TriggerClientEvent(citizen, $"igi:{vehicle.VehicleType().Name}:claim", JsonConvert.SerializeObject(vehicle));
		}

		private static void LoadNearbyVehicles([FromSource] Player citizen, string charJson)
		{
			Character character = JsonConvert.DeserializeObject<Character>(charJson);

			Server.Log($"Checking nearby vehicles to spawn to position {character.Position.ToString()}");

			foreach (Vehicle vehicle in Server.Db.Vehicles.Where(v => v.Handle == null).ToArray())
			{
				Server.Log($"Checking {vehicle.Id} with position {vehicle.Position}");
				if (!(Vector3.Distance(vehicle.Position, character.Position) < VehicleLoadDistance)) continue;

				Server.Log($"Spawning vehicle to {citizen.Name} via event: 'igi:{vehicle.VehicleType().Name}:spawn'");
				BaseScript.TriggerClientEvent(citizen, $"igi:{vehicle.VehicleType().Name}:spawn", JsonConvert.SerializeObject(vehicle));
			}
		}
	}
}
