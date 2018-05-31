using System;
using System.Linq;
using IgiCore.Models.Player;
using IgiCore.SDK.Server.Rpc;
using IgiCore.Server.Storage;

namespace IgiCore.Server.Rpc
{
	public class RpcEvent : IRpcEvent
	{
		private readonly Lazy<User> user;

		public string Event { get; }

		public IClient Client { get; }

		public User User => this.user.Value;

		public RpcEvent(string @event, IClient client)
		{
			this.Event = @event;
			this.Client = client;

			this.user = new Lazy<User>(() =>
			{
				using (var context = new StorageContext())
				{
					context.Configuration.ProxyCreationEnabled = false;
					context.Configuration.LazyLoadingEnabled = false;

					return context.Users.Single(u => u.SteamId == this.Client.SteamId);
				}
			});
		}

		public void Reply(params object[] payloads)
		{
			this.Client.Event(this.Event).Trigger(payloads);
		}
	}
}
