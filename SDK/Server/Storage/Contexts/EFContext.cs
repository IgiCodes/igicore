using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using IgiCore.SDK.Server.Configuration;
using MySql.Data.EntityFramework;

namespace IgiCore.SDK.Server.Storage.Contexts
{
	[DbConfigurationType(typeof(MySqlEFConfiguration))]
	public abstract class EFContext<T> : DbContext where T : DbContext
	{
		protected EFContext() : base(ServerConfiguration.DatabaseConnection)
		{
			Database.SetInitializer(new MigrateDatabaseToLatestVersion<T, Configuration<T>>());
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder
				.Properties()
				.Where(x => x.PropertyType == typeof(string) && !x.GetCustomAttributes(false).OfType<ColumnAttribute>().Any(q => q.TypeName != null && q.TypeName.Equals("varchar", StringComparison.InvariantCultureIgnoreCase)))
				.Configure(c => c.HasColumnType("varchar"));
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
