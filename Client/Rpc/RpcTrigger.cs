using CitizenFX.Core;
using IgiCore.Core.Rpc;

namespace IgiCore.Client.Rpc
{
	public class RpcTrigger : IRpcTrigger
	{
		public void Fire(RpcMessage message)
		{
			Client.Log($"Fire: \"{message.Event}\" with {message.Payloads.Count} payload(s):");

			BaseScript.TriggerServerEvent(message.Event, message.Build());
		}
	}
}
