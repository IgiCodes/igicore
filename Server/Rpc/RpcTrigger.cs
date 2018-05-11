using CitizenFX.Core;
using IgiCore.Core.Rpc;

namespace IgiCore.Server.Rpc
{
	public class RpcTrigger : IRpcTrigger
	{
		public void Fire(RpcMessage message)
		{
			Server.Log($"Fire: \"{message.Event}\" with {message.Payloads.Count} payload(s):");

			if (message.Target != null)
			{
				message.Target.TriggerEvent(message.Event, message.Build());
			}
			else
			{
				BaseScript.TriggerClientEvent(message.Event, message.Build());
			}
		}
	}
}
