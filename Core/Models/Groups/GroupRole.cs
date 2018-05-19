using System;
using System.Collections.Generic;
using IgiCore.Core.Extensions;

namespace IgiCore.Core.Models.Groups
{
	public class GroupRole : Model, IGroupRole
	{
		public string Name { get; set; }
		public int Rank { get; set; }
        public virtual Group Group { get; set; }
		public virtual List<GroupMember> Members { get; set; }

		public GroupRole()
		{
			this.Id = GuidGenerator.GenerateTimeBasedGuid();
			this.Name = string.Empty;
			this.Rank = 0;
			this.Created = DateTime.UtcNow;
		}
	}
}
