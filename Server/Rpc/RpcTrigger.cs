﻿using CitizenFX.Core;
using IgiCore.SDK.Core.Rpc;
using IgiCore.Server.Diagnostics;

namespace IgiCore.Server.Rpc
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

			if (message.Target != null)
			{
				new PlayerList()[message.Target.Handle].TriggerEvent(message.Event, this.serializer.Serialize(message));
			}
			else
			{
				BaseScript.TriggerClientEvent(message.Event, this.serializer.Serialize(message));
			}
		}
	}
}