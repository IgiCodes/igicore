using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using IgiCore.Core.Models.Objects.Vehicles;
using Vehicle = IgiCore.Core.Models.Objects.Vehicles.Vehicle;
using Newtonsoft.Json;
using static CitizenFX.Core.Native.API;

namespace IgiCore.Client.Services
{
    public class VehicleService : ClientService
    {
        public List<Tuple<Type, int>> Tracked { get; set; } = new List<Tuple<Type, int>>();

        private const int VehicleLoadDistance = 500;

        public override async Task OnTick(Client client)
        {
            Update(client);
            Save();

            await BaseScript.Delay(1000);
        }

        private void Update(Client client)
        {
            foreach (Tuple<Type, int>trackedVehicle in this.Tracked.ToList())
            {
                int vehicleHandle = NetToVeh(trackedVehicle.Item2);
                var citVeh = new CitizenFX.Core.Vehicle(vehicleHandle);
                var player = new Player(API.GetNearestPlayerToEntity(citVeh.Handle));
                //Debug.WriteLine($"Nearest Player: {player.Name}");

                if (player == client.LocalPlayer || !NetworkIsPlayerConnected(player.Handle))
                {
                    if (Vector3.Distance(client.LocalPlayer.Character.Position, citVeh.Position) > VehicleLoadDistance)
                    {
                        citVeh.Delete();
                        this.Tracked.Remove(trackedVehicle);
                        BaseScript.TriggerServerEvent("igi:car:unclaim", trackedVehicle.Item2);
                    }
                }
                else
                {
                    int netId = NetworkGetNetworkIdFromEntity(citVeh.Handle);
                    //Debug.WriteLine($"Vehicle: {vehicleHandle} NetId: {netId} - {citVeh.Position}");

                    Car car = citVeh;
                    car.NetId = netId;
                    // Transfer the vehicle to the closest client
                    //Client.Log($"Removing Vehicle from tracked: {car.Handle}");
                    //this.Tracked.Remove(car.Handle ?? 0);
                    Client.Log($"Transfering vehicle to player: {player.ServerId}  -  {car.Handle}");
                    BaseScript.TriggerServerEvent("igi:car:transfer", JsonConvert.SerializeObject(car),
                        player.ServerId);
                }
            }
        }

        private void Save()
        {
            Client.Log("Save called.");
            Client.Log(string.Join(", ", this.Tracked));
            foreach (Tuple<Type, int> trackedVehicle in this.Tracked)
            {
                int vehicleHandle = NetToVeh(trackedVehicle.Item2);
                var citVeh = new CitizenFX.Core.Vehicle(vehicleHandle);
                int netId = NetworkGetNetworkIdFromEntity(citVeh.Handle);
                Debug.WriteLine($"Saving Vehicle: {trackedVehicle.Item2} - {citVeh.Position}");

                // NOTE: vehicle won't have its ID
                Vehicle vehicle = citVeh;
                vehicle.TrackingUserId = Client.User.Id;
                vehicle.NetId = netId;
                vehicle.Hash = citVeh.Model.Hash;
                string className = trackedVehicle.Item1.BaseType.IsSubclassOf(typeof(Vehicle))
                    ? trackedVehicle.Item1.BaseType.Name
                    : trackedVehicle.Item1.Name;


                switch (className)
                {
                    case "Car":
                        Car car = (Car)vehicle;
                        // Add car specific props...
                        BaseScript.TriggerServerEvent($"igi:car:save", JsonConvert.SerializeObject(car));
                        break;
                    default:
                        BaseScript.TriggerServerEvent($"igi:{className}:save", JsonConvert.SerializeObject(vehicle, trackedVehicle.Item1, new JsonSerializerSettings()));
                        break;
                }
                

                
            }
        }
    }
}
