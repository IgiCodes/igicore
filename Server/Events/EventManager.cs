using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IgiCore.SDK.Server.Events;

namespace IgiCore.Server.Events
{
	public class EventManager : IEventManager
	{
		private readonly Dictionary<string, List<Subscription>> subscriptions = new Dictionary<string, List<Subscription>>();

		public void On<T>(string @event, Action<T> action)
		{
			lock (this.subscriptions)
			{
				if (!this.subscriptions.ContainsKey(@event))
				{
					this.subscriptions.Add(@event, new List<Subscription>());
				}

				this.subscriptions[@event].Add(new Subscription(action));
			}
		}

		public void Raise<T>(string @event, T message)
		{
			lock (this.subscriptions)
			{
				if (!this.subscriptions.ContainsKey(@event)) return;
				
				foreach (var subscription in this.subscriptions[@event])
				{
					subscription.Handle(message);
				}
			}
		}

		public async Task RaiseAsync<T>(string @event, T message)
		{
			await Task.Factory.StartNew(() =>
			{
				Raise(@event, message);
			});
		}

		public class Subscription
		{
			private readonly object handler;

			public Subscription(object handler)
			{
				this.handler = handler;
			}

			public bool Handle<T>(T message)
			{
				bool cancel = false;

				(this.handler as Action<T>)(message);

				return cancel;
			}
		}
	}
}
