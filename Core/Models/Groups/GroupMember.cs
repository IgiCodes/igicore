using System;
using System.Collections.Generic;
using IgiCore.Core.Extensions;
using IgiCore.Core.Models.Economy;
using IgiCore.Core.Models.Player;
using JetBrains.Annotations;

namespace IgiCore.Core.Models.Groups
{
	public class GroupMember : Model, IGroupMember
    {
		public virtual Character Character { get; set; }
		public virtual List<GroupRole> Roles { get; set; }
        public virtual Group Group { get; set; }
        public virtual List<Salary> Salaries { get; set; }

		public GroupMember()
		{
			this.Id = GuidGenerator.GenerateTimeBasedGuid();
			this.Created = DateTime.UtcNow;
		}
	}
}
