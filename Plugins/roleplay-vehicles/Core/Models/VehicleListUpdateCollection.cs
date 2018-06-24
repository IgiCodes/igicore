using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using IgiCore.SDK.Core.Models;

namespace Roleplay.Vehicles.Core.Models
{
	public class VehicleListUpdateCollection
	{
		public List<DeltaUpdate<Vehicle>> Vehicles { get; set; } = new List<DeltaUpdate<Vehicle>>();
		public List<DeltaUpdate<Car>> Cars { get; set; } = new List<DeltaUpdate<Car>>();
		public List<DeltaUpdate<Bike>> Bikes { get; set; } = new List<DeltaUpdate<Bike>>();
		//public List<DeltaUpdate<Boat>> Boats { get; set; } = new List<DeltaUpdate<Boat>>;
		//public List<DeltaUpdate<Plane>> Planes { get; set; } = new List<DeltaUpdate<Plane>>;
		//public List<DeltaUpdate<Helicopter>> Helicopters { get; set; } = new List<DeltaUpdate<Helicopter>>;
	}
}
