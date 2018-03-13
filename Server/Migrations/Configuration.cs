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

        protected override void Seed(Storage.MySql.DB context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
