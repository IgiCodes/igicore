using System;
using System.Collections.Generic;
using IgiCore.Core.Extensions;
using IgiCore.Core.Models.Groups;
using IgiCore.Server.Models.Player;
using Citizen = CitizenFX.Core.Player;

namespace IgiCore.Server.Models.Groups
{
    public class Group : IGroup
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime? Deleted { get; set; }

        public virtual List<GroupRole> Roles { get; set; }
        public virtual List<GroupMember> Members { get; set; }

        public Group() { this.Id = GuidGenerator.GenerateTimeBasedGuid(); }

        public static Group Create(Character owner, string name)
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
            Server.Db.SaveChanges();

            return group;
        }

    }
}
