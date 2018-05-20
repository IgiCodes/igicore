using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IgiCore.Client.Controllers.Objects.Vehicles;
using IgiCore.Client.Controllers.Player;
using IgiCore.Client.Events;
using IgiCore.Client.Models;
using IgiCore.Client.Rpc;
using IgiCore.Core.Models.Connection;
using IgiCore.Core.Models.Objects.Vehicles;
using IgiCore.Core.Rpc;

namespace IgiCore.Client.Controllers
{
	public class ClientController : Controller
	{
		public event EventHandler<ServerInformationEventArgs> OnClientReady;

		/// <summary>
		/// Loads initial data from the server, raises events and attaches handlers.
		/// </summary>
		public async Task Startup()
		{
			Client.Log("Startup");

			// Load server details
			this.OnClientReady?.Invoke(this, new ServerInformationEventArgs(await Server
				.Event(RpcEvents.GetServerInformation)
				.Request<ServerInformation>()
			));
			// Load user
			Client.Instance.Controllers.First<UserController>().Load();
			// Load user's characters
			Client.Instance.Controllers.First<CharacterController>().GetCharacters();

			Client.Log("Waiting for character selection...");
		}
	}
}
