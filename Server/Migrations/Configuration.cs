using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using IgiCore.Server.Managers;
using IgiCore.Server.Storage.MySql;

namespace IgiCore.Server.Migrations
{
	internal sealed class Configuration : DbMigrationsConfiguration<DB>
	{
		public Configuration()
		{
			this.TargetDatabase = new DbConnectionInfo(ConfigurationManager.Configuration.Database.ToString(), "MySql.Data.MySqlClient");

#if true
			this.AutomaticMigrationsEnabled = true;
#else
			this.AutomaticMigrationsEnabled = false;
#endif
		}

		protected override void Seed(DB context)
		{
			//  This method will be called after migrating to the latest version.

			//  You can use the DbSet<T>.AddOrUpdate() helper extension method 
			//  to avoid creating duplicate seed data.
		}
	}
}
