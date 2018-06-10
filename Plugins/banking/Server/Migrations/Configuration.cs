using System;
using System.Linq;
using Banking.Core.Models;
using Banking.Server.Storage;
using IgiCore.SDK.Server.Migrations;

namespace Banking.Server.Migrations
{
	internal sealed class Configuration : MigrationConfiguration<BankingContext>
	{
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
