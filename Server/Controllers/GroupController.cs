using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IgiCore.Models.Groups;
using IgiCore.Models.Player;
using IgiCore.Server.Extentions;
using IgiCore.Server.Models.Player;

namespace IgiCore.Server.Controllers
{
    public static class GroupController
    {
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
