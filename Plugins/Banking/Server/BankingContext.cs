using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using Banking.Core.Models;
using Banking.Server.Migrations;
using IgiCore.Models.Player;
using IgiCore.SDK.Server.Storage.Contexts;
using MySql.Data.EntityFramework;

namespace Banking.Server
{
	[DbConfigurationType(typeof(MySqlEFConfiguration))]
	public class BankingContext : DbContext//EFContext<BankingContext>
	{
		public virtual DbSet<Bank> Banks { get; set; }
		public virtual DbSet<BankBranch> BankBranches { get; set; }
		public virtual DbSet<BankAtm> BankAtms { get; set; }
		public virtual DbSet<BankAccount> BankAccounts { get; set; }
		public virtual DbSet<BankAccountCard> BankAccountCards { get; set; }
		public virtual DbSet<BankAccountMember> BankAccountMembers { get; set; }

		public BankingContext() : base("server=harvest;Port=3306;Database=fivem;User Id=root;Password=password;CharSet=utf8mb4;SSL Mode=None")
		{
			Database.SetInitializer(new MigrateDatabaseToLatestVersion<BankingContext, Migrations.Configuration>());
			//Database.SetInitializer<BankingContext>(null);
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder
				.Properties()
				.Where(x => x.PropertyType == typeof(string) && !x.GetCustomAttributes(false).OfType<ColumnAttribute>().Any(q => q.TypeName != null && q.TypeName.Equals("varchar", StringComparison.InvariantCultureIgnoreCase)))
				.Configure(c => c.HasColumnType("varchar"));
		}

	}
}
