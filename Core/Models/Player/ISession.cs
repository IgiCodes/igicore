using System;

namespace IgiCore.Core.Models.Player
{
	public interface ISession
	{
		Guid Id { get; set; }
		string IpAddress { get; set; }
		DateTime Connected { get; set; }
		DateTime? Disconnected { get; set; }
		string DisconnectReason { get; set; }
	}
}
