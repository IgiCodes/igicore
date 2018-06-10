using System.Data.Entity;
using System.Data.Entity.Migrations;
using JetBrains.Annotations;

namespace IgiCore.SDK.Server.Migrations
{
	[PublicAPI]
	public abstract class MigrationConfiguration<TContext> : DbMigrationsConfiguration<TContext> where TContext : DbContext
	{
		protected MigrationConfiguration()
		{
			this.AutomaticMigrationsEnabled = false;
			this.AutomaticMigrationDataLossAllowed = false;
		}

		protected override void Seed(TContext context) { }
	}
}
