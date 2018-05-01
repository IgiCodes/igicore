using System;

namespace IgiCore.Core.Models.Player
{
	public interface IUser
	{
		Guid Id { get; set; }
		string SteamId { get; set; }
		string Name { get; set; }
		DateTime? AcceptedRules { get; set; }
		DateTime Created { get; set; }
	}
}
