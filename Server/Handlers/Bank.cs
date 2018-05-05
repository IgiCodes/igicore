using System;
using System.Data.Entity.Migrations;
using System.Linq;
using CitizenFX.Core;
using IgiCore.Core;
using IgiCore.Core.Models.Economy.Banking;
using IgiCore.Server.Models.Economy.Banking;
using IgiCore.Server.Models.Player;
using Newtonsoft.Json;
using Citizen = CitizenFX.Core.Player;

namespace IgiCore.Server.Handlers
{
    public static class Bank
    {
        public static void AtmWithdraw([FromSource] Citizen citizen, string atmIdString, string memberIdString, string amountString )
        {
            Server.Log($"AtmWithdraw called: {atmIdString}  {memberIdString}  {amountString}");
            try
            {
                Guid atmId = JsonConvert.DeserializeObject<Guid>(atmIdString);
                BankAtm atm = Server.Db.BankATMs.First(a => a.Id == atmId);
                Guid memberId = JsonConvert.DeserializeObject<Guid>(memberIdString);
                BankAccountMember member = Server.Db.BankAccountMembers.First(m => m.Id == memberId);
                double amount = JsonConvert.DeserializeObject<double>(amountString);

                member.Account.Balance -= amount;

                Server.Db.BankAccountMembers.AddOrUpdate(member);
                Server.Db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Server.Log($"AtmWithdraw Failed: {e.Message}");
                BaseScript.TriggerClientEvent(RpcEvents.BankAtmWithdraw, JsonConvert.SerializeObject(false));
                return;
            }

            BaseScript.TriggerClientEvent(RpcEvents.BankAtmWithdraw, JsonConvert.SerializeObject(true));
        }
    }
}
