using System;
using IgiCore.Core.Rpc;

namespace IgiCore.Client.Rpc
{
	public class RpcRequest : Core.Rpc.RpcRequest
	{
		public RpcRequest(string @event, IRpcHandler handler, IRpcTrigger trigger, IRpcSerializer serializer) : base(@event, handler, trigger, serializer) { }

		public void On(Action action)
		{
			this.RpcHandler.Attach(this.Message.Event, action);
		}
		public void On<T>(Action<T> action)
		{
			this.RpcHandler.Attach(this.Message.Event, new Action<string>(j =>
			{
				var message = this.RpcSerializer.Deserialize<RpcMessage>(j);

				action(this.RpcSerializer.Deserialize<T>(message.Payloads[0]));
			}));
		}
		public void On<T1, T2>(Action<T1, T2> action)
		{
			this.RpcHandler.Attach(this.Message.Event, new Action<string>(j =>
			{
				var message = this.RpcSerializer.Deserialize<RpcMessage>(j);

				action(
					this.RpcSerializer.Deserialize<T1>(message.Payloads[0]),
					this.RpcSerializer.Deserialize<T2>(message.Payloads[1])
				);
			}));
		}
		public void On<T1, T2, T3>(Action<T1, T2, T3> action)
		{
			this.RpcHandler.Attach(this.Message.Event, new Action<string>(j =>
			{
				var message = this.RpcSerializer.Deserialize<RpcMessage>(j);
				action(
					this.RpcSerializer.Deserialize<T1>(message.Payloads[0]),
					this.RpcSerializer.Deserialize<T2>(message.Payloads[1]),
					this.RpcSerializer.Deserialize<T3>(message.Payloads[2])
				);
			}));
		}
		public void On<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action)
		{
			this.RpcHandler.Attach(this.Message.Event, new Action<string>(j =>
			{
				var message = this.RpcSerializer.Deserialize<RpcMessage>(j);
				action(
					this.RpcSerializer.Deserialize<T1>(message.Payloads[0]),
					this.RpcSerializer.Deserialize<T2>(message.Payloads[1]),
					this.RpcSerializer.Deserialize<T3>(message.Payloads[2]),
					this.RpcSerializer.Deserialize<T4>(message.Payloads[3])
				);
			}));
		}
		public void On<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> action)
		{
			this.RpcHandler.Attach(this.Message.Event, new Action<string>(j =>
			{
				var message = this.RpcSerializer.Deserialize<RpcMessage>(j);
				action(
					this.RpcSerializer.Deserialize<T1>(message.Payloads[0]),
					this.RpcSerializer.Deserialize<T2>(message.Payloads[1]),
					this.RpcSerializer.Deserialize<T3>(message.Payloads[2]),
					this.RpcSerializer.Deserialize<T4>(message.Payloads[3]),
					this.RpcSerializer.Deserialize<T5>(message.Payloads[4])
				);
			}));
		}
	}
}
