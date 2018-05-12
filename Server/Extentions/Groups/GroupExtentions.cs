using System;
using System.Linq;
using System.Threading.Tasks;
using IgiCore.Core.Exceptions;
using IgiCore.Core.Models.Groups;
using IgiCore.Core.Models.Player;

namespace IgiCore.Server.Extentions.Groups
{
    public static class GroupExtentions
    {
        public static async Task Delete(this Group group)
        {
            group.Members.ForEach(async m => await m.SoftDelete());
            group.Roles.ForEach(async r => await r.SoftDelete());
            await group.SoftDelete();
        }

        public static async Task<GroupRole> CreateRole(this Group group, string name, int rank = 100)
        {
            Group dbGroup = Server.Db.Groups.NotDeleted().FirstOrDefault(g => g.Id == group.Id);
            if (dbGroup == null) throw new GroupException($"Argument {nameof(group)} was not found in the Groups DbSet.");
            GroupRole role = new GroupRole
            {
                Name = name,
                Rank = rank,
                Created = DateTime.UtcNow
            };
            dbGroup.Roles.Add(role);
            await Server.Db.SaveChangesAsync();
            return role;
        }

        public static async Task<GroupMember> CreateMember(this Group group, Character character)
        {
            Group dbGroup = Server.Db.Groups.NotDeleted().FirstOrDefault(g => g.Id == group.Id);
            if (dbGroup == null) throw new ArgumentNullException(nameof(group));
            GroupMember member = new GroupMember
            {
                Character = character
            };
            dbGroup.Members.Add(member);
            await Server.Db.SaveChangesAsync();
            return member;
        }
        
    }
}
