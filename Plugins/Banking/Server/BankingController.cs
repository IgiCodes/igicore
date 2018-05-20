using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Banking.Core.Models;
using IgiCore.Models.Player;
using IgiCore.SDK.Core.Diagnostics;
using IgiCore.SDK.Server;
using IgiCore.SDK.Server.Configuration;
using IgiCore.SDK.Server.Rpc;

namespace Banking.Server
{
	public class BankingController : ServerController
	{
		public BankingController(ILogger logger, IServerEventsManager serverEvents, IClientEventsManager clientEvents, IConfiguration configuration) : base(logger, serverEvents, clientEvents, configuration)
		{
			this.ServerEvents.On("character:create", new Action<Character>(OnCharacterCreate));

			//clientEvents.On<Guid, Guid, double>("igi:bank:atm:withdraw", AtmWithdraw);

			using (var context = new BankingContext())
			{
				if (context.Banks.Any()) return;

				context.Banks.Add(new Bank
				{
					Id = Guid.NewGuid(),
					Name = "Test"
				});

				context.SaveChanges();
			}
		}

		public async void OnCharacterCreate(Character character)
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
							//Member = character
						}
					}
				};

				bank.Accounts.Add(account);

				await context.SaveChangesAsync();

				this.ServerEvents.Trigger("bank:account:created", character, account);
			}
		}

		public async void AtmWithdraw(ClientEvent client, Guid atmId, Guid memberId, double amount)
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
