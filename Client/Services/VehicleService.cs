using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using IgiCore.Core.Extensions;
using IgiCore.Core.Models.Objects.Vehicles;
using Newtonsoft.Json;
using static CitizenFX.Core.Native.API;
using Vehicle = CitizenFX.Core.Vehicle;

namespace IgiCore.Client.Services
{
    public class VehicleService : ClientService
    {
        private const int VehicleLoadDistance = 500;

        public List<Tuple<Type, int>> Tracked { get; set; } = new List<Tuple<Type, int>>();

        public override async Task OnTick(Client client)
        {
            Update(client);
            Save();

            await BaseScript.Delay(1000);
        }

        private void Update(Client client)
        {
            foreach (var trackedVehicle in this.Tracked.ToList())
            {
                var vehicleHandle = NetToVeh(trackedVehicle.Item2);
                Vehicle citVeh = new Vehicle(vehicleHandle);
                Player player = new Player(GetNearestPlayerToEntity(citVeh.Handle));

                if (player == client.LocalPlayer || !NetworkIsPlayerConnected(player.Handle))
                {
                    if (!(Vector3.Distance(client.LocalPlayer.Character.Position, citVeh.Position) >
                          VehicleLoadDistance)) continue;

                    citVeh.Delete();
                    this.Tracked.Remove(trackedVehicle);
                    BaseScript.TriggerServerEvent("igi:car:unclaim", trackedVehicle.Item2);
                }
                else
                {
                    var netId = NetworkGetNetworkIdFromEntity(citVeh.Handle);
                    //Debug.WriteLine($"Vehicle: {vehicleHandle} NetId: {netId} - {citVeh.Position}");

                    Car car = citVeh;
                    car.NetId = netId;

                    // Transfer the vehicle to the closest client
                    //Client.Log($"Removing Vehicle from tracked: {car.Handle}");
                    //this.Tracked.Remove(car.Handle ?? 0);

                    Client.Log($"Transfering vehicle to player: {player.ServerId}  -  {car.Handle}");
                    BaseScript.TriggerServerEvent(
                        "igi:car:transfer",
                        JsonConvert.SerializeObject(car),
                        player.ServerId);
                }
            }
        }

        private void Save()
        {
            Client.Log("Save called.");
            Client.Log(string.Join(", ", this.Tracked));

            foreach (var trackedVehicle in this.Tracked)
            {
                var vehicleHandle = NetToVeh(trackedVehicle.Item2);
                Vehicle citVeh = new Vehicle(vehicleHandle);
                var netId = NetworkGetNetworkIdFromEntity(citVeh.Handle);

                Debug.WriteLine($"Saving Vehicle: {trackedVehicle.Item2} - {citVeh.Position}");

                Core.Models.Objects.Vehicles.Vehicle vehicle = citVeh;
                vehicle.TrackingUserId = Client.User.Id;
                vehicle.NetId = netId;
                vehicle.Hash = citVeh.Model.Hash;

                switch (trackedVehicle.Item1.VehicleType().Name)
                {
                    case "Car":
                        Car car = (Car) vehicle;
                        // Add car specific props...
                        BaseScript.TriggerServerEvent(
                            $"igi:{trackedVehicle.Item1.VehicleType().Name}:save",
                            JsonConvert.SerializeObject(car));
                        break;

                    default:
                        BaseScript.TriggerServerEvent(
                            $"igi:{trackedVehicle.Item1.VehicleType().Name}:save",
                            JsonConvert.SerializeObject(vehicle, trackedVehicle.Item1, new JsonSerializerSettings()));
                        break;
                }
            }
        }
    }
}
