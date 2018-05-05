using System;
using System.Collections.Generic;
using IgiCore.Core.Extensions;
using IgiCore.Core.Models.Groups;
using IgiCore.Server.Models.Player;

namespace IgiCore.Server.Models.Groups
{
	public class GroupMember : IGroupMember
	{
		public Guid Id { get; set; }
		public DateTime Created { get; set; } = DateTime.UtcNow;
		public DateTime? Deleted { get; set; }
		public virtual Character Character { get; set; }
		public virtual List<GroupRole> Roles { get; set; }

		public GroupMember() { this.Id = GuidGenerator.GenerateTimeBasedGuid(); }
	}
}
