using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IgiCore.Core.Extensions;
using IgiCore.Core.Models.Groups;
using IgiCore.Server.Models.Player;

namespace IgiCore.Server.Models.Groups
{
	public class Group : IGroup
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public DateTime Created { get; set; }
		public DateTime? Deleted { get; set; }

		public virtual List<GroupRole> Roles { get; set; }
		public virtual List<GroupMember> Members { get; set; }

		public Group()
		{
			this.Id = GuidGenerator.GenerateTimeBasedGuid();
			this.Name = string.Empty;
			this.Created = DateTime.UtcNow;
		}

		public static async Task<Group> Create(Character owner, string name)
		{
			List<GroupRole> roles = new List<GroupRole>
			{
				new GroupRole
				{
					Name = "Owner",
					Rank = 1
				},
				new GroupRole
				{
					Name = "Member",
					Rank = 100
				}
			};

			Group group = new Group
			{
				Name = name,
				Roles = roles,
				Members = new List<GroupMember>
				{
					new GroupMember
					{
						Character = owner,
						Roles = roles
					}
				}
			};

			Server.Db.Groups.Add(group);
			await Server.Db.SaveChangesAsync();

			return group;
		}
	}
}
