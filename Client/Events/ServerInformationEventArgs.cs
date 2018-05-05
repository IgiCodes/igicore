using System;
using IgiCore.Core.Models.Connection;

namespace IgiCore.Client.Events
{
	public class ServerInformationEventArgs : EventArgs
	{
		public ServerInformation Information { get; }

		public ServerInformationEventArgs(ServerInformation information)
		{
			this.Information = information;
		}
	}
}
