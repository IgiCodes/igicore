using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using IgiCore.Core.Models.Objects.Vehicles;
using Newtonsoft.Json;

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
            foreach (int vehicleHandle in this.Tracked)
            {
                var citVeh = new CitizenFX.Core.Vehicle(vehicleHandle);
                var player = new Player(API.GetNearestPlayerToEntity(citVeh.Handle));
                Debug.WriteLine($"Nearest Player: {player.Name}");

                if (player != client.LocalPlayer)
                {
                    // Transfer the vehicle to the closest client
                }
            }
        }

        private void Save()
        {
            foreach (int vehicleHandle in this.Tracked)
            {
                var citVeh = new CitizenFX.Core.Vehicle(vehicleHandle);
                Debug.WriteLine($"Vehicle: {vehicleHandle} - {citVeh.Position}");

                Car car = citVeh;
                // NOTE: car won't have its ID
                
                BaseScript.TriggerServerEvent("igi:vehicle:save", JsonConvert.SerializeObject(car));
            }
        }
    }
}
