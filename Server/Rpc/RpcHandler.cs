using System;
using System.Linq;
using IgiCore.SDK.Core.Rpc;

namespace IgiCore.Server.Rpc
{
	public class RpcHandler : IRpcHandler
	{
		public void Attach(string @event, Delegate callback)
		{
			//Server.Log($"Attach: \"{@event}\" {callback.Method.Name}({string.Join(", ", callback.Method.GetParameters().Select(p => p.ParameterType + " " + p.Name))})");

			Server.Instance.EventHandlers[@event] += callback;
		}

		public void Detach(string @event, Delegate callback)
		{
			//Server.Log($"Detach: \"{@event}\" {callback.Method.Name}({string.Join(", ", callback.Method.GetParameters().Select(p => p.ParameterType + " " + p.Name))})");

			Server.Instance.EventHandlers[@event] -= callback;
		}
	}
}
