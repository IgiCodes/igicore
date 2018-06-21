using System.Collections.Generic;

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
	}
}
