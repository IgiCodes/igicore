using System.Data.Entity;
using Banking.Core.Models;
using IgiCore.SDK.Server.Storage;

namespace Banking.Server.Storage
{
	public class BankingContext : EFContext<BankingContext>
	{
		public DbSet<Bank> Banks { get; set; }

		public DbSet<BankBranch> BankBranches { get; set; }

		public DbSet<BankAtm> BankAtms { get; set; }

		public DbSet<BankAccount> BankAccounts { get; set; }

		public DbSet<BankAccountCard> BankAccountCards { get; set; }

		public DbSet<BankAccountMember> BankAccountMembers { get; set; }
	}
}
