using System.Collections.Generic;
using System.Threading.Tasks;
using Citizen = CitizenFX.Core.Player;

namespace IgiCore.Core.Commands
{
	public abstract class Command : ICommand
	{

		private string Name { get; set; }

		public Command(string name)
		{
			this.Name = name;
		}

		public abstract void RunCommand(Citizen citizen, List<string> args);

		public string getName()
		{
			return this.Name;
		}

	}
}
