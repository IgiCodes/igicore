using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IgiCore.Core.Models.Objects.Vehicles;

namespace IgiCore.Server.Storage.Contexts
{
	public class VehicleContext : Context
	{
		public DbSet<Vehicle> Vehicles { get; set; }
		public DbSet<Car> Cars { get; set; }
		public DbSet<Bike> Bikes { get; set; }

		public VehicleContext()
		{
			this.Database.Log = m => Server.Log(m);
		}
	}
}
