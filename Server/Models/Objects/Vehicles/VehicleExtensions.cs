using System.ComponentModel.DataAnnotations.Schema;
using CitizenFX.Core;
using IgiCore.Core.Models.Objects.Vehicles;
using IgiCore.Server.Storage.MySql;

namespace IgiCore.Server.Models.Objects.Vehicles
{
    public static class VehicleExtensions
    {
        public static void Save(Car car)
        {
            Debug.WriteLine($"Saving vehicle: {car.Hash}");
        }
    }
}