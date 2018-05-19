using System;
using IgiCore.Client.Events;
using IgiCore.Client.Models;
using IgiCore.Client.Rpc;
using IgiCore.Core.Controllers;
using IgiCore.Core.Rpc;

namespace IgiCore.Client.Controllers.Player
{
	public class UserController : Controller
	{
		public event EventHandler<UserEventArgs> OnUserLoaded;

		/// <summary>
		/// Gets or sets the currently loaded user.
		/// </summary>
		/// <value>
		/// The loaded user.
		/// </value>
		public User User { get; protected set; }

		public async void Load()
		{
			// Load user
			this.User = await Server
				.Event(RpcEvents.GetUser)
				.Request<User>();

			this.OnUserLoaded?.Invoke(this, new UserEventArgs(this.User));
		}
	}
}
