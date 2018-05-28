using CitizenFX.Core;
using IgiCore.Client.Diagnostics;
using IgiCore.SDK.Core.Rpc;

namespace IgiCore.Client.Rpc
{
	public class RpcTrigger
	{
		private readonly Logger logger;
		private readonly Serializer serializer;

		public RpcTrigger(Logger logger, Serializer serializer)
		{
			this.logger = logger;
			this.serializer = serializer;
		}

		public void Fire(OutboundMessage message)
		{
			this.logger.Debug($"Fire: \"{message.Event}\" with {message.Payloads.Count} payload(s): {string.Join(", ", message.Payloads)}");

			BaseScript.TriggerServerEvent(message.Event, this.serializer.Serialize(message));
		}
	}
}
