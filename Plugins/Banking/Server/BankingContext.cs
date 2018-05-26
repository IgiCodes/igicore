using System.Data.Entity;
using Banking.Core.Models;
using Banking.Server.Migrations;
using IgiCore.SDK.Server.Storage.Contexts;

namespace Banking.Server
{
	public class BankingContext : EFContext
	{
		public virtual DbSet<Bank> Banks { get; set; }
		public virtual DbSet<BankBranch> BankBranches { get; set; }
		public virtual DbSet<BankAtm> BankAtms { get; set; }
		public virtual DbSet<BankAccount> BankAccounts { get; set; }
		public virtual DbSet<BankAccountCard> BankAccountCards { get; set; }
		public virtual DbSet<BankAccountMember> BankAccountMembers { get; set; }

		public BankingContext()
		{
			Database.SetInitializer(new MigrateDatabaseToLatestVersion<BankingContext, Configuration>());
		}
	}
}
