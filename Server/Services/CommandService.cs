using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using IgiCore.SDK.Core.Diagnostics;
using IgiCore.Server.Commands;
using IgiCore.Server.Storage.MySql;

namespace IgiCore.Server.Services
{
	public class CommandService : ServerService
	{
		private readonly ILogger logger;

		protected readonly List<Command> Commands = new List<Command>();

		public CommandService(ILogger logger)
		{
			this.logger = logger;

			Rpc.Client
				.Event("chatMessage")
				.On(OnChatMessage);
		}

		public override async Task Initialize()
		{
			Register(new GpsCommand());
			Register(new ComponentCommand());
			Register(new PropCommand());
			Register(new CarCommand());
			Register(new BikeCommand());
			//Register(new GroupCommand());
		}

		public void Register(Command command)
		{
			this.Commands.Add(command);
		}

		protected async void OnChatMessage(int playerId, string playerName, string message)
		{
			Player player = Server.Instance.Players[playerId];

		    if (!message.StartsWith("/")) return;

            var args = message.Split(' ').ToList();
			var name = args.First().ToLowerInvariant();
			args = args.Skip(1).ToList();

		    var command = this.Commands.FirstOrDefault(c => $"/{c.Name.ToLowerInvariant()}" == name);

            if (command == null)
			{
				this.logger.Log($"Unknown command /{name}");
				return;
			}

			this.logger.Log($"/{name} command called");

			await command.RunCommand(player, args);
		}
	}
}
