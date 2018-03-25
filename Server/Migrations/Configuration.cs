namespace IgiCore.Server.Migrations
{
	using System.Data.Entity.Migrations;

	internal sealed class Configuration : DbMigrationsConfiguration<Storage.MySql.DB>
	{
		public Configuration()
		{
			this.TargetDatabase = new System.Data.Entity.Infrastructure.DbConnectionInfo(Config.MySqlConnString, "MySql.Data.MySqlClient");
			this.AutomaticMigrationsEnabled = true;
		}
	}
}
