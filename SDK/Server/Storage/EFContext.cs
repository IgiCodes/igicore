using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using IgiCore.SDK.Server.Configuration;
using JetBrains.Annotations;
using MySql.Data.EntityFramework;

namespace IgiCore.SDK.Server.Storage
{
	[PublicAPI]
	[DbConfigurationType(typeof(MySqlEFConfiguration))]
	public abstract class EFContext<TContext> : DbContext where TContext : DbContext
	{
		static EFContext()
		{
			Database.SetInitializer<TContext>(null);
		}

		protected EFContext() : base(ServerConfiguration.DatabaseConnection) { }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder
				.Properties()
				.Where(x => x.PropertyType == typeof(string) && !x.GetCustomAttributes(false).OfType<ColumnAttribute>().Any(q => q.TypeName != null && q.TypeName.Equals("varchar", StringComparison.InvariantCultureIgnoreCase)))
				.Configure(c => c.HasColumnType("varchar"));
		}
	}
}
