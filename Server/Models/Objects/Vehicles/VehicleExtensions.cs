using CitizenFX.Core;
using IgiCore.Core.Models.Objects.Vehicles;
using System.Data.Entity.Migrations;

namespace IgiCore.Server.Models.Objects.Vehicles
{
    public static class VehicleExtensions
    {
        public static void Save(Car car)
        {
            Debug.WriteLine($"Saving vehicle {car.Id} {car.Handle}");

	        Server.Db.Cars.AddOrUpdate(car);
	        Server.Db.SaveChanges();
		}
    }
}
