using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IgiCore.Core.Models.Economy.Banking;
using IgiCore.Core.Models.Player;
using IgiCore.Server.Models.Economy.Banking;
using IgiCore.Server.Models.Player;

namespace IgiCore.Server.Services.Economy
{
	public class BankService : ServerService
	{
		public override void Initialize() { }

		public override async Task<Character> OnCharacterCreate(Character character)
		{
			var bank = Server.Db.Banks.FirstOrDefault();

			if (bank == null) return character;

			bank.Accounts.Add(new BankAccount
			{
				Balance = 10000,
				AccountNumber = 123454321,
				Type = BankAccountTypes.Personal,
				Members = new List<BankAccountMember>
				{
					new BankAccountMember
					{
						Member = character
					}
				}
			});
			await Server.Db.SaveChangesAsync();

			return await base.OnCharacterCreate(character);
		}
	}
}
