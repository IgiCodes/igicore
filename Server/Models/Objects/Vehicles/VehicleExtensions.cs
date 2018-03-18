using System;
using CitizenFX.Core;
using IgiCore.Core.Models.Objects.Vehicles;
using System.Data.Entity.Migrations;
using System.Linq;

namespace IgiCore.Server.Models.Objects.Vehicles
{
	public static class VehicleExtensions
	{
		public static void Save(Car car) // Has no ID
		{
			Server.Log($"Given car {car.Id} {car.Handle}");

			if (car.Id == Guid.Empty)
			{
				car.Id = Server.Db.Cars.First(c => c.Handle == car.Handle).Id;
			}

			Server.Log($"Looked up {car.Id} {car.Handle}");

			Server.Db.Cars.AddOrUpdate(car);
			Server.Db.SaveChanges();
		}
	}
}
