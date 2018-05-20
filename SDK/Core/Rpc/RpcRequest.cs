using System;
using System.Threading.Tasks;

namespace IgiCore.SDK.Core.Rpc
{
	public class RpcRequest
	{
		protected readonly RpcMessage Message = new RpcMessage();
		protected readonly IRpcHandler RpcHandler;
		protected readonly IRpcTrigger RpcTrigger;
		protected readonly IRpcSerializer RpcSerializer;
		
		public RpcRequest(string @event, IRpcHandler handler, IRpcTrigger trigger, IRpcSerializer serializer)
		{
			this.Message.Event = @event;
			this.RpcHandler = handler;
			this.RpcTrigger = trigger;
			this.RpcSerializer = serializer;
		}

		public RpcRequest Attach<T>(T data)
		{
			this.Message.Payloads.Add(this.RpcSerializer.Serialize(data));

			return this;
		}

		public void Trigger()
		{
			this.RpcTrigger.Fire(this.Message);
		}

		public async Task<T> Request<T>()
		{
			var results = await Request();

			return this.RpcSerializer.Deserialize<T>(results.Payloads[0]);
		}

		public async Task<Tuple<T1, T2>> Request<T1, T2>()
		{
			var results = await Request();

			return new Tuple<T1, T2>(
				this.RpcSerializer.Deserialize<T1>(results.Payloads[0]),
				this.RpcSerializer.Deserialize<T2>(results.Payloads[1])
			);
		}

		public async Task<Tuple<T1, T2, T3>> Request<T1, T2, T3>()
		{
			var results = await Request();

			return new Tuple<T1, T2, T3>(
				this.RpcSerializer.Deserialize<T1>(results.Payloads[0]),
				this.RpcSerializer.Deserialize<T2>(results.Payloads[1]),
				this.RpcSerializer.Deserialize<T3>(results.Payloads[2])
			);
		}

		public async Task<Tuple<T1, T2, T3, T4>> Request<T1, T2, T3, T4>()
		{
			var results = await Request();

			return new Tuple<T1, T2, T3, T4>(
				this.RpcSerializer.Deserialize<T1>(results.Payloads[0]),
				this.RpcSerializer.Deserialize<T2>(results.Payloads[1]),
				this.RpcSerializer.Deserialize<T3>(results.Payloads[2]),
				this.RpcSerializer.Deserialize<T4>(results.Payloads[3])
			);
		}

		public async Task<Tuple<T1, T2, T3, T4, T5>> Request<T1, T2, T3, T4, T5>()
		{
			var results = await Request();

			return new Tuple<T1, T2, T3, T4, T5>(
				this.RpcSerializer.Deserialize<T1>(results.Payloads[0]),
				this.RpcSerializer.Deserialize<T2>(results.Payloads[1]),
				this.RpcSerializer.Deserialize<T3>(results.Payloads[2]),
				this.RpcSerializer.Deserialize<T4>(results.Payloads[3]),
				this.RpcSerializer.Deserialize<T5>(results.Payloads[4])
			);
		}

		protected async Task<RpcMessage> Request()
		{
			var tcs = new TaskCompletionSource<RpcMessage>();
			var handler = new Action<string>(json =>
			{
				var message = this.RpcSerializer.Deserialize<RpcMessage>(json);

				tcs.SetResult(message);
			});

			try
			{
				this.RpcHandler.Attach(this.Message.Event, handler);

				this.Trigger();
				
				return await tcs.Task;
			}
			finally
			{
				this.RpcHandler.Detach(this.Message.Event, handler);
			}
		}
	}
}
