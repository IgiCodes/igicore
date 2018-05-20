using System;
using CitizenFX.Core;
using IgiCore.SDK.Core.Rpc;

namespace IgiCore.Server.Rpc
{
	public class RpcRequest : SDK.Core.Rpc.RpcRequest
	{
		public RpcRequest(string @event, IRpcHandler handler, IRpcTrigger trigger, IRpcSerializer serializer) : base(@event, handler, trigger, serializer) { }

		public RpcRequest Target(Player player)
		{
			//this.Message.Target = player;

			return this;
		}

		public void On(Action action)
		{
			this.RpcHandler.Attach(this.Message.Event, action);
		}
		public void On(Action<Player> action)
		{
			this.RpcHandler.Attach(this.Message.Event, action);
		}
		public void On(Action<Player, string> action)
		{
			this.RpcHandler.Attach(this.Message.Event, action);
		}
		public void On(Action<Player, string, CallbackDelegate> action)
		{
			this.RpcHandler.Attach(this.Message.Event, action);
		}
		public void On(Action<int, string, string> action)
		{
			this.RpcHandler.Attach(this.Message.Event, action);
		}
		public void On<T>(Action<T> action)
		{
			this.RpcHandler.Attach(this.Message.Event, action);
		}
	}
}
