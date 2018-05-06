using IgiCore.Core.Services;
using System.Collections.Generic;
using Citizen = CitizenFX.Core.Player;
using IgiCore.Core.Commands;
using System.Linq;
using System.Threading.Tasks;
using IgiCore.Server.Commands;

namespace IgiCore.Server.Services
{
	public class CommandService : Service, IService
	{

		private static List<Command> cmds = new List<Command>();

		public CommandService()
		{
			HandleEvent<int, string, string>("chatMessage", OnChatMessage);
		}

		public override void Initialise()
		{
			registerCommands();
		}

		public static void registerCommands()
		{
			cmds.Add(new GPSCommand());
		}

		public async void OnChatMessage(int playerId, string playerName, string message)
		{
			Citizen citizen = Server.Instance.Players[playerId];

			var args = message.Split(' ').ToList();
			var name = args.First().ToLowerInvariant();
			args = args.Skip(1).ToList();

			Command cmd = null;

			foreach (Command command in cmds)
			{
				if (command.getName() == name)
				{
					cmd = command;
				}
			}

			if (cmd != null)
			{

				await Task.Run(() => {
					cmd.RunCommand(citizen, args);
				});

			} else
			{
				await Task.Run(() => {
					Server.Log("Unknown command " + name);
				});
			}
		}

	}
}
