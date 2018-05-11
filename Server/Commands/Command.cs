using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace IgiCore.Server.Commands
{
	public abstract class Command
	{
		public abstract string Name { get; }

		public abstract Task RunCommand(Player player, List<string> args);
	}
}
