using System;
using System.Collections.Generic;
using System.Data.Entity;
using Banking.Core.Models;
using IgiCore.Models.Player;
using IgiCore.SDK.Core;
using IgiCore.SDK.Core.Diagnostics;
using IgiCore.SDK.Server;
using IgiCore.SDK.Server.Rpc;

namespace Banking.Server
{
	public class BankingController : ConfigurableController<BankingConfiguration>
	{
		public BankingController(ILogger logger, IEventsManager events, BankingConfiguration configuration) : base(logger, events, configuration)
		{
			this.Events.Event("character:create").On<Character>(OnCharacterCreate);
			this.Events.Event("igi:bank:atm:withdraw").On<Guid, Guid, double>(AtmWithdraw);

			this.Logger.Log(this.Configuration.Test);
		}

		public async void OnCharacterCreate(Client client, Character character)
		{
			using (var context = new BankingContext())
			{
				var bank = await context.Banks.FirstOrDefaultAsync();

				if (bank == null) return;

				var account = new BankAccount
				{
					Balance = 10000,
					AccountNumber = "0123456787901234",
					Type = BankAccountTypes.Personal,
					Members = new List<BankAccountMember>
					{
						new BankAccountMember
						{
							Member = character
						}
					}
				};

				bank.Accounts.Add(account);

				await context.SaveChangesAsync();

				//this.Events.Trigger("bank:account:created", character, account);
			}
		}

		public async void AtmWithdraw(Client client, Guid atmId, Guid memberId, double amount)
		{
			try
			{
				using (var context = new BankingContext())
				{
					BankAtm atm = await context.BankAtms.SingleAsync(a => a.Id == atmId);

					BankAccountMember member = await context.BankAccountMembers.SingleAsync(m => m.Id == memberId);
					member.Account.Balance -= amount;

					await context.SaveChangesAsync();
				}

				//client
				//	.Reply()
				//	.Attach(true)
				//	.Trigger();
			}
			catch (Exception ex)
			{
				this.Logger.Error(ex);

				//client
				//	.Reply()
				//	.Attach(false)
				//	.Trigger();
			}
		}
	}
}
