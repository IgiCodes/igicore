using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IgiCore.Core.Models.Economy.Banking;
using IgiCore.Server.Models.Economy.Banking;
using IgiCore.Server.Models.Player;
using IgiCore.Server.Storage.MySql;

namespace IgiCore.Server.Services.Economy
{
    public class BankService : ServerService
    {
        public override void Initialise() { }

        public override async Task<Character> OnCharacterCreate(Character character)
        {
            Server.Db.Banks.First().Accounts.Add(new BankAccount
            {
                Balance = 10000,
                AccountNumber = 123454321,
                Type = BankAccountTypes.Personal,
                Members = new List<BankAccountMember>
                {
                    new BankAccountMember
                    {
                        Id = Guid.Parse("e9286e6f-e74d-4510-855b-5318ef0f71af"),
                        Member = character
                    }
                }
            });
            await Server.Db.SaveChangesAsync();

            return await base.OnCharacterCreate(character);
        }
    }
}
