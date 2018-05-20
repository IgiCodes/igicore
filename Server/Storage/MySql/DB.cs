using System;
using System.Data.Entity;
using IgiCore.Models.Appearance;
using IgiCore.Models.Groups;
using IgiCore.Models.Player;
using IgiCore.SDK.Server.Storage;
using IgiCore.Server.Migrations;
using MySql.Data.Entity;

namespace IgiCore.Server.Storage.MySql
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class DB : DbContext
    {
        public DbSet<Session> Sessions { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Character> Characters { get; set; }

        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupMember> GroupMembers { get; set; }
        public DbSet<GroupRole> GroupRoles { get; set; }

        public DbSet<Style> Styles { get; set; }

        //public DbSet<Vehicle> Vehicles { get; set; }
        //public DbSet<Car> Cars { get; set; }
        //public DbSet<Bike> Bikes { get; set; }

        //public DbSet<Inventory> Inventories { get; set; }

        public DB() : base(Config.MySqlConnString)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DB, Configuration>());

            //this.Database.Log = m => Server.Log(m);
        }

		//protected override void OnModelCreating(DbModelBuilder modelBuilder)
		//{
		//	modelBuilder.Entity<Car>();

		//	var entityMethod = typeof(DbModelBuilder).GetMethod("Entity");

		//	foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
		//	{
		//		var entityTypes = assembly
		//			.GetTypes()
		//			.Where(t =>
		//				t.GetCustomAttributes(typeof(PersistentAttribute), inherit: true)
		//					.Any());

		//		foreach (var type in entityTypes)
		//		{
		//			entityMethod.MakeGenericMethod(type)
		//				.Invoke(modelBuilder, new object[] { });
		//		}
		//	}
		//}
	}
}
