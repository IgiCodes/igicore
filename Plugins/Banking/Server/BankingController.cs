using System;
using System.Collections.Generic;
using System.Data.Entity;
using Banking.Core.Models;
using IgiCore.Models.Player;
using IgiCore.SDK.Core.Diagnostics;
using IgiCore.SDK.Server.Controllers;
using IgiCore.SDK.Server.Rpc;

namespace Banking.Server
{
	public class BankingController : ConfigurableController<BankingConfiguration>
	{
		public BankingController(ILogger logger, IRpcHandler rpc, BankingConfiguration configuration) : base(logger, rpc, configuration)
		{
			this.Rpc.Event("character:create").On<Character>(OnCharacterCreate);
			this.Rpc.Event("igi:bank:atm:withdraw").On<Guid, Guid, double>(AtmWithdraw);

			this.Logger.Log(this.Configuration.Test);
		}

		public async void OnCharacterCreate(IRpcEvent e, Character character)
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

				e.Client.Event("bank:account:created").Trigger(character, account);
			}
		}

		public async void AtmWithdraw(IRpcEvent e, Guid atmId, Guid memberId, double amount)
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

				e.Reply(true);
			}
			catch (Exception ex)
			{
				this.Logger.Error(ex);

				e.Reply(false);
			}
		}
	}
}
