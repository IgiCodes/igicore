using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using IgiCore.SDK.Server.Configuration;

namespace IgiCore.SDK.Server.Storage.Contexts
{
	public abstract class EFContext<T> : DbContext where T : DbContext
	{
		protected EFContext() : base(ServerConfiguration.DatabaseConnection)
		{
			Database.SetInitializer(new MigrateDatabaseToLatestVersion<T, Configuration<T>>());
		}
	}

	internal class Configuration<T> : DbMigrationsConfiguration<T> where T : DbContext
	{
		public Configuration()
		{
			this.AutomaticMigrationDataLossAllowed = true;
			this.AutomaticMigrationsEnabled = true;
		}
	}
}
