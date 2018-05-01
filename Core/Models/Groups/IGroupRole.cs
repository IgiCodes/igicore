using System;

namespace IgiCore.Core.Models.Groups
{
	public interface IGroupRole
	{
		Guid Id { get; set; }
		string Name { get; set; }
		int Rank { get; set; }
		DateTime Created { get; set; }
		DateTime? Deleted { get; set; }
	}
}
