using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using IgiCore.Core.Models.Objects.Vehicles;
using Newtonsoft.Json;
using static CitizenFX.Core.Native.API;

namespace IgiCore.Client.Services
{
    public class VehicleService : Service
    {
        public List<int> Tracked { get; set; } = new List<int>();

        public override async Task OnTick(Client client)
        {
            Update(client);
            Save();

            await BaseScript.Delay(1000);
        }

        private void Update(Client client)
        {
            foreach (int vehicleHandle in this.Tracked.ToList())
            {
                var citVeh = new CitizenFX.Core.Vehicle(vehicleHandle);
                var player = new Player(API.GetNearestPlayerToEntity(citVeh.Handle));
                //Debug.WriteLine($"Nearest Player: {player.Name}");

                if (player == client.LocalPlayer) continue;
                int netId = NetworkGetNetworkIdFromEntity(citVeh.Handle);
                //Debug.WriteLine($"Vehicle: {vehicleHandle} NetId: {netId} - {citVeh.Position}");

                Car car = citVeh;
                car.NetId = netId;
                // Transfer the vehicle to the closest client
                //Client.Log($"Removing Vehicle from tracked: {car.Handle}");
                this.Tracked.Remove(car.Handle);
                Client.Log($"Transfering vehicle to player: {player.ServerId}  -  {car.Handle}");
                BaseScript.TriggerServerEvent("igi:vehicle:transfer", JsonConvert.SerializeObject(car), player.ServerId);
            }
        }

        private void Save()
        {
            foreach (int vehicleHandle in this.Tracked)
            {
                var citVeh = new CitizenFX.Core.Vehicle(vehicleHandle);
                int netId = NetworkGetNetworkIdFromEntity(citVeh.Handle);
                //Debug.WriteLine($"Vehicle: {vehicleHandle} - {citVeh.Position}");

                Car car = citVeh;
                car.NetId = netId;

                // NOTE: car won't have its ID

                BaseScript.TriggerServerEvent("igi:vehicle:save", JsonConvert.SerializeObject(car));
            }
        }
    }
}
