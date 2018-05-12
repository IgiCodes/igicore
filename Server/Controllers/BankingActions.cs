using System;
using System.Data.Entity.Migrations;
using System.Linq;
using CitizenFX.Core;
using IgiCore.Core.Models.Economy.Banking;
using IgiCore.Core.Rpc;
using IgiCore.Server.Models.Economy.Banking;
using IgiCore.Server.Rpc;

namespace IgiCore.Server.Controllers
{
	public static class BankingActions
	{
		public static void AtmWithdraw([FromSource] Player player, string json)
		{
			var response = RpcResponse<Guid, Guid, double>.Parse(json);

			Server.Log($"AtmWithdraw called: {response.Result1}  {response.Result2}  {response.Result3}");

			try
			{
				BankAtm atm = Server.Db.BankAtms.First(a => a.Id == response.Result1);
				BankAccountMember member = Server.Db.BankAccountMembers.First(m => m.Id == response.Result2);
				double amount = response.Result3;

				member.Account.Balance -= amount;

				Server.Db.BankAccountMembers.AddOrUpdate(member);
				Server.Db.SaveChangesAsync();

				player
					.Event(RpcEvents.BankAtmWithdraw)
					.Attach(true)
					.Trigger();
			}
			catch (Exception e)
			{
				Server.Log($"AtmWithdraw Failed: {e.Message}");

				player
					.Event(RpcEvents.BankAtmWithdraw)
					.Attach(false)
					.Trigger();
			}
		}
	}
}
