using System;
using System.Linq;
using System.Threading.Tasks;
using IgiCore.Core.Models.Groups;

namespace IgiCore.Server.Extentions.Groups
{
    public static class RoleExtentions
    {
        public static async Task AddMember(this GroupRole role, GroupMember member) => await member.AddRole(role);

        public static async Task Delete(this GroupRole role)
        {
            Group dbGroup = Server.Db.Groups.NotDeleted().FirstOrDefault(g => g.Id == role.Group.Id);
            if (dbGroup == null) throw new ArgumentNullException(nameof(role));
            dbGroup.Members.ForEach(m => m.Roles.Remove(role));
            await Server.Db.SaveChangesAsync();
            await role.SoftDelete();
        }
    }
}
