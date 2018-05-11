using System;
using System.Linq;
using IgiCore.Core.Rpc;

namespace IgiCore.Client.Rpc
{
	public class RpcHandler : IRpcHandler
	{
		public void Attach(string @event, Delegate callback)
		{
			Client.Log($"Attach: \"{@event}\" {callback.Method.Name}({string.Join(", ", callback.Method.GetParameters().Select(p => p.ParameterType + " " + p.Name))})");

			Client.Instance.Handlers[@event] += callback;
		}

		public void Detach(string @event, Delegate callback)
		{
			Client.Log($"Detach: \"{@event}\" {callback.Method.Name}({string.Join(", ", callback.Method.GetParameters().Select(p => p.ParameterType + " " + p.Name))})");

			Client.Instance.Handlers[@event] -= callback;
		}
	}
}
