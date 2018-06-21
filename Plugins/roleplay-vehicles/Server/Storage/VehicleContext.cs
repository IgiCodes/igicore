using System.Data.Entity;
using IgiCore.SDK.Server.Storage;
using Roleplay.Vehicles.Core.Models;

namespace Roleplay.Vehicles.Server.Storage
{
	public class VehicleContext : EFContext<VehicleContext>
	{
		public DbSet<Vehicle> Vehicles { get; set; }
		public DbSet<Car> Cars { get; set; }
		public DbSet<VehicleExtra> VehicleExtras { get; set; }
		public DbSet<VehicleWheel> VehicleWheels { get; set; }
		public DbSet<VehicleWindow> VehicleWindows { get; set; }
		public DbSet<VehicleMod> VehicleMods { get; set; }
		public DbSet<VehicleDoor> VehicleDoors { get; set; }
		public DbSet<VehicleSeat> VehicleSeats { get; set; }
	}
}
