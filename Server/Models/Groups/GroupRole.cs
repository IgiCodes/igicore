using System;
using System.Collections.Generic;
using IgiCore.Core.Extensions;
using IgiCore.Core.Models.Groups;

namespace IgiCore.Server.Models.Groups
{
	public class GroupRole : IGroupRole
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public int Rank { get; set; }
		public DateTime Created { get; set; } = DateTime.UtcNow;
		public DateTime? Deleted { get; set; }

		public virtual List<GroupMember> Members { get; set; }

		public GroupRole()
		{
			this.Id = GuidGenerator.GenerateTimeBasedGuid();
		}
	}
}
