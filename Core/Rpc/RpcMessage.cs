using System;
using System.Collections.Generic;
using CitizenFX.Core;
using Newtonsoft.Json;

namespace IgiCore.Core.Rpc
{
	public class RpcMessage
	{
		public string Event { get; set; }
		public List<string> Payloads { get; set; } = new List<string>();
		public Player Target { get; set; } = null;
		public DateTime Created { get; set; } = DateTime.UtcNow;
		public DateTime Sent { get; set; }

		public string Build() => JsonConvert.SerializeObject(this);
	}
}
