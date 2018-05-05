using System;

namespace IgiCore.Core.Models.Economy.Banking
{
	public interface IBankAccountMembers
	{
		Guid Id { get; set; }
		DateTime Created { get; set; }
		DateTime? Deleted { get; set; }
	}
}
