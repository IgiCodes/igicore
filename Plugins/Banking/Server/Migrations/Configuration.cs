using System.Data.Entity.Migrations;

namespace Banking.Server.Migrations
{
	internal sealed class Configuration : DbMigrationsConfiguration<BankingContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BankingContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
