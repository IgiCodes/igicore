using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using IgiCore.Core.Commands;

namespace IgiCore.Server.Commands
{
	public class GPSCommand : Command
	{

		public GPSCommand() : base("/gps")
		{

		}

		public override void RunCommand(Player citizen, List<string> args)
		{
			Server.Log("/gps command called");
			Server.TriggerClientEvent(citizen, "igi:user:gps");
		}

	}
}
