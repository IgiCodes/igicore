using System;
using System.Collections.Generic;
using CitizenFX.Core;

namespace IgiCore.Server.Rpc
{
	public class OutboundMessage
	{
		public Player Target { get; set; } = null;

		public string Event { get; set; }

		public List<string> Payloads { get; set; } = new List<string>();

		public DateTime Created { get; set; } = DateTime.UtcNow;
	}
}
