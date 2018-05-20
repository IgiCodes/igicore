using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using IgiCore.Core.Extensions;
using IgiCore.Core.Models.Objects.Vehicles;
using IgiCore.Core.Rpc;
using IgiCore.Models.Player;
using IgiCore.SDK.Diagnostics;
using IgiCore.Server.Rpc;
using IgiCore.Server.Storage.Contexts;
using IgiCore.Server.Storage.MySql;
using Newtonsoft.Json;
using User = IgiCore.Server.Models.Player.User;

namespace IgiCore.Server.Services
{
	public class VehicleService : ServerService
	{
		private const int VehicleLoadDistance = 500;

		private readonly ILogger logger;

		public VehicleService(ILogger logger)
		{
			this.logger = logger;

			Client.Event(RpcEvents.CharacterSave).On(LoadNearbyVehicles);
			Client.Event(ServerEvents.PlayerDropped).On(ReassignTrackedVehicles);
		}

		public override async Task Initialize()
		{
			this.logger.Log("VehicleService.Initialize called");

			using (var context = new VehicleContext())
			using (var transaction = context.Database.BeginTransaction())
			{
				foreach (var vehicle in context.Repository<Vehicle>())
				{
					vehicle.Handle = null;
					vehicle.NetId = null;
					vehicle.TrackingUserId = Guid.Empty;
				}

				await context.SaveChangesAsync();

				transaction.Commit();
			}
		}

		private async void ReassignTrackedVehicles([FromSource] Player disconnectedCitizen, string disconnectMessage, CallbackDelegate kickReason)
		{
			this.logger.Log("ReassignTrackedVehicles called");

			var disconnectedSteamId = disconnectedCitizen.Identifiers["steam"];
			this.logger.Log($"Disconnected user steam id: {disconnectedSteamId}");

			using (var entities = new DB())
			{
				User disconnectedUser = entities.Users.FirstOrDefault(u => u.SteamId == disconnectedSteamId);
				if (disconnectedUser == null) return;
				this.logger.Log($"Reassigning tracked vehicles for disconnected player: {disconnectedUser.Name}");

				var vehicles = entities.Vehicles.Where(v => v.TrackingUserId == disconnectedUser.Id).ToList();
				if (!vehicles.Any()) return;

				this.logger.Log("Vehicles found, looking up online users");

				var users = new List<User>();
				foreach (Player serverPlayer in Server.Instance.Players)
				{
					var steamId = serverPlayer.Identifiers["steam"];
					if (steamId == disconnectedSteamId) continue;

					users.Add(entities.Users.First(u => u.SteamId == steamId));
				}

				this.logger.Log("Looping through vehicles to assign players");

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
								if (Vector3.Distance(character.Position, vehicle.Position) <
									Vector3.Distance(assignedVehicles[vehicle].Item2.Position, vehicle.Position))
									assignedVehicles[vehicle] = new Tuple<User, Character>(user, character);
							}
						}
					}
				}

				if (!assignedVehicles.Any())
				{
					this.logger.Log("No vehicles assigned to anyone");

					Player hostClient = null;
					try
					{
						hostClient = Server.Instance.Players.First(p => p.Handle == API.GetHostId());
					}
					catch (Exception ex)
					{
						this.logger.Log(ex.Message);
					}

					foreach (Vehicle vehicle in vehicles)
					{
						if (hostClient != null)
							BaseScript.TriggerClientEvent(hostClient, "igi:entity:delete", vehicle.NetId, vehicle.Hash);

						vehicle.NetId = null;
						vehicle.Handle = null;
						vehicle.TrackingUserId = Guid.Empty;

						entities.Vehicles.AddOrUpdate(vehicle);
						await entities.SaveChangesAsync();
					}
				}

				foreach (var assignedVehicle in assignedVehicles)
				{
					Player citizen = Server.Instance.Players.First(c => c.Identifiers["steam"] == entities.Users.First(u => u.Id == assignedVehicle.Value.Item1.Id).SteamId);

					AssignVehicle(assignedVehicle.Key, citizen);
				}
			}
		}

		private void AssignVehicle(IVehicle vehicle, Player citizen)
		{
			this.logger.Log($"Assinging vehicle to {citizen.Name} via event: 'igi:{vehicle.VehicleType().Name}:claim'");

			BaseScript.TriggerClientEvent(citizen, $"igi:{vehicle.VehicleType().Name}:claim", JsonConvert.SerializeObject(vehicle));
		}

		private async void LoadNearbyVehicles([FromSource] Player citizen, string charJson)
		{
			Character character = JsonConvert.DeserializeObject<Character>(charJson);

			this.logger.Log($"Checking nearby vehicles to spawn to position {character.Position.ToString()}");

			using (var entities = new DB())
			{
				foreach (Vehicle vehicle in await entities.Vehicles.Where(v => v.Handle == null).ToListAsync())
				{
					this.logger.Log($"Checking {vehicle.Id} with position {vehicle.Position}");
					if (!(Vector3.Distance(vehicle.Position, character.Position) < VehicleLoadDistance)) continue;

					this.logger.Log($"Spawning vehicle to {citizen.Name} via event: 'igi:{vehicle.VehicleType().Name}:spawn'");
					BaseScript.TriggerClientEvent(citizen, $"igi:{vehicle.VehicleType().Name}:spawn", JsonConvert.SerializeObject(vehicle));
				}
			}
		}
	}
}
