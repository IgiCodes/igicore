using System;
using CitizenFX.Core;
using IgiCore.Core.Models.Objects.Vehicles;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IgiCore.Server.Models.Objects.Vehicles
{
	public static class VehicleExtensions
	{
		public static void Save(Car car) // Has no ID
		{
			//Server.Log($"Given car {car.Id} {car.Handle
		    if (car.Id == Guid.Empty) car.Id = Server.Db.Cars.FirstOrDefault(c => c.Handle == car.Handle)?.Id ?? Guid.Empty;
		    if (car.Id == Guid.Empty) return;
            //Server.Log($"Looked up {car.Id} {car.Handle
            Server.Db.Cars.AddOrUpdate(car);
			Server.Db.SaveChanges();
		}

	}
}
