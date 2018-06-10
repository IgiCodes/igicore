using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Banking.Core.Models;
using Banking.Server.Storage;
using IgiCore.SDK.Core.Diagnostics;
using IgiCore.SDK.Server.Controllers;
using IgiCore.SDK.Server.Events;
using IgiCore.SDK.Server.Rpc;
using Roleplay.Core.Models.Player;

namespace Banking.Server.Controllers
{
	public class BankingController : ConfigurableController<Configuration>
	{
		public BankingController(ILogger logger, IEventManager events, IRpcHandler rpc, Configuration configuration) : base(logger, events, rpc, configuration)
		{
			this.Rpc.Event("character:create").On<Character>(OnCharacterCreate);
			this.Rpc.Event("igi:bank:atm:withdraw").On<Guid, Guid, double>(AtmWithdraw);
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
