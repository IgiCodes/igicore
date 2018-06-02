using System.Collections.Generic;
using System.Linq;
using CitizenFX.Core;
using IgiCore.Server.Commands;

namespace IgiCore.Server.Services
{
	public class CommandService : ServerService
	{
		protected readonly List<Command> Commands = new List<Command>();

		public CommandService()
		{
			Rpc.Client
				.Event("chatMessage")
				.On(OnChatMessage);
		}

		public override void Initialize()
		{
			Register(new GpsCommand());
			Register(new ComponentCommand());
			Register(new PropCommand());
			Register(new CarCommand());
			Register(new BikeCommand());
			Register(new GroupCommand());
			Register(new ReviveCommand());
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
				Server.Log($"Unknown command /{name}");
				return;
			}

			Server.Log($"/{name} command called");

			await command.RunCommand(player, args);
		}
	}
}
