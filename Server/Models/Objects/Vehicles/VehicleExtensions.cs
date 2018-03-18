using CitizenFX.Core;
using IgiCore.Core.Models.Objects.Vehicles;

namespace IgiCore.Server.Models.Objects.Vehicles
{
    public static class VehicleExtensions
    {
        public static void Save(Car car)
        {
            Debug.WriteLine($"Saving vehicle {car.Id} {car.Handle}");
        }
    }
}
