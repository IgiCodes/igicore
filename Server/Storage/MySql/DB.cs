using IgiCore.Core.Models.Appearance;
using IgiCore.Core.Models.Objects.Vehicles;
using IgiCore.Server.Migrations;
using IgiCore.Server.Models;
using MySql.Data.Entity;
using System.Data.Entity;

namespace IgiCore.Server.Storage.MySql
{
	[DbConfigurationType(typeof(MySqlEFConfiguration))]
	public class DB : DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<Character> Characters { get; set; }
		public DbSet<Style> Styles { get; set; }
		public DbSet<Vehicle> Vehicles { get; set; }
		public DbSet<Car> Cars { get; set; }
		public DbSet<Bike> Bikes { get; set; }

		public DB() : base(Config.MySqlConnString)
		{
			Database.SetInitializer(new MigrateDatabaseToLatestVersion<DB, Configuration>());

			//this.Database.Log = m => Server.Log(m);
		}
	}
}
