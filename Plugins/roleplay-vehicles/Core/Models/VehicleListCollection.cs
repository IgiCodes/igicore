using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Roleplay.Vehicles.Core.Extensions;

namespace Roleplay.Vehicles.Core.Models
{
	public class VehicleListCollection
	{
		public List<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
		public List<Car> Cars { get; set; } = new List<Car>();
		public List<Bike> Bikes { get; set; } = new List<Bike>();
		//public List<Boat> Boats { get; set; } = new List<Boat>();
		//public List<Plane> Planes { get; set; } = new List<Plane>();
		//public List<Helicopter> Helicopters { get; set; } = new List<Helicopter>();

		public IList Set<T>() where T : Vehicle
		{
			switch (typeof(T).VehicleType().Name)
			{
				case "Car":
					return Cars;
				case "Bike":
					return Bikes;
				case "Vehicle":
					return Vehicles;
				default:
					return new List<T>();
			}
		}
	}
}
