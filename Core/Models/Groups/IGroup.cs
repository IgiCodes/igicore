using System;

namespace IgiCore.Core.Models.Groups
{
	public interface IGroup
	{
		Guid Id { get; set; }
		string Name { get; set; }
		DateTime Created { get; set; }
		DateTime? Deleted { get; set; }
	}
}
