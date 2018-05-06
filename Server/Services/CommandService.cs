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
			HandleEvent<int, string, string>("chatMessage", OnChatMessage);
		}

		public override void Initialise()
		{
			this.Register(new GpsCommand());
			this.Register(new ComponentCommand());
			this.Register(new PropCommand());
			this.Register(new CarCommand());
			this.Register(new BikeCommand());
			this.Register(new GroupCommand());
		}

		public void Register(Command command)
		{
			this.Commands.Add(command);
		}

		protected async void OnChatMessage(int playerId, string playerName, string message)
		{
			Player player = Server.Instance.Players[playerId];

			var args = message.Split(' ').ToList();
			var name = args.First().ToLowerInvariant();
			args = args.Skip(1).ToList();

			var command = this.Commands.FirstOrDefault(c => c.Name.ToLowerInvariant() == name);

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
