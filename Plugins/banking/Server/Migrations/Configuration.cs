using System;
using System.Data.Entity.Migrations;
using System.Linq;
using Banking.Core.Models;

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
			if (context.Banks.Any()) return;

			context.Banks.Add(new Bank
			{
				Id = Guid.NewGuid(),
				Name = "Example Bank"
			});

			context.SaveChanges();
		}
	}
}
