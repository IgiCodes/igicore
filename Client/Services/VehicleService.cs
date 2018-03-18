using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using IgiCore.Core.Models.Objects.Vehicles;
using Vehicle = IgiCore.Core.Models.Objects.Vehicles.Vehicle;
using Newtonsoft.Json;

namespace IgiCore.Client.Services
{
    public class VehicleService : Service
    {
        public List<int> Tracked { get; set; } = new List<int>();

        public override async Task OnTick(Client client)
        {
            Update(client);
            Save(client);
            await BaseScript.Delay(1000);
        }

        private void Update(Client client)
        {
            foreach (int vehicleHandle in Tracked)
            {
                var citVeh = new CitizenFX.Core.Vehicle(vehicleHandle);
                Player player = new Player(API.GetNearestPlayerToEntity(citVeh.Handle));
                Debug.WriteLine($"Nearest Player: {player.Name}");

                if (player != client.LocalPlayer)
                {
                    // Transfer the vehicle to the closest client
                }
            }
        }

        private void Save(Client client)
        {
            foreach (int vehicleHandle in Tracked)
            {
                var citVeh = new CitizenFX.Core.Vehicle(vehicleHandle);
                Debug.WriteLine($"Vehicle: {vehicleHandle} - {citVeh.Position.ToString()}");

                Car car = citVeh;
                
                BaseScript.TriggerServerEvent("igi:vehicle:save", JsonConvert.SerializeObject(car));
            }
        }


    }
}