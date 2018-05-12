using System;
using System.Collections.Generic;
using IgiCore.Core.Extensions;

namespace IgiCore.Core.Models.Groups
{
	public class Group : Model, IGroup
	{
		public string Name { get; set; }

		public virtual List<GroupRole> Roles { get; set; }
		public virtual List<GroupMember> Members { get; set; }

		public Group()
		{
			this.Id = GuidGenerator.GenerateTimeBasedGuid();
			this.Name = string.Empty;
			this.Created = DateTime.UtcNow;
		}

	}
}
