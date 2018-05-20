using System.Data.Entity.Migrations;
using IgiCore.Server.Storage.MySql;

namespace IgiCore.Server.Migrations
{
	internal sealed class Configuration : DbMigrationsConfiguration<DB>
	{
		public Configuration()
		{
			this.AutomaticMigrationDataLossAllowed = true;
			this.AutomaticMigrationsEnabled = true; ;
		}
    }
}
