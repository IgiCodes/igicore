using System.Data.Entity;
using Banking.Core.Models;
using Banking.Server.Migrations;
using IgiCore.SDK.Server.Storage.Contexts;

namespace Banking.Server
{
	public class BankingContext : EFContext
	{
		public DbSet<Bank> Banks { get; set; }

		public DbSet<BankBranch> BankBranches { get; set; }

		public DbSet<BankAtm> BankAtms { get; set; }

		public DbSet<BankAccount> BankAccounts { get; set; }

		public DbSet<BankAccountCard> BankAccountCards { get; set; }

		public DbSet<BankAccountMember> BankAccountMembers { get; set; }

		public BankingContext()
		{
			Database.SetInitializer(new MigrateDatabaseToLatestVersion<BankingContext, Configuration>());
		}
	}
}
