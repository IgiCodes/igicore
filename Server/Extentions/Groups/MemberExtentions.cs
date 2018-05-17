using System;
using System.Linq;
using System.Threading.Tasks;
using IgiCore.Core.Models.Economy;
using IgiCore.Core.Models.Groups;

namespace IgiCore.Server.Extentions.Groups
{
    public static class MemberExtentions
    {
        public static async Task Delete(this GroupMember member) => await member.SoftDelete();

        public static async Task AddRole(this GroupMember member, GroupRole role)
        {
            GroupMember dbMember = Server.Db.GroupMembers.NotDeleted().FirstOrDefault(r => r.Id == member.Id);
            if (dbMember == null) throw new ArgumentNullException(nameof(member));
            dbMember.Roles.Add(role);
            await Server.Db.SaveChangesAsync();
        }

	    public static async Task AddSalary(this GroupMember member, Salary salary)
	    {
		    GroupMember dbMember = Server.Db.GroupMembers.NotDeleted().FirstOrDefault(r => r.Id == member.Id);
		    if (dbMember == null) throw new ArgumentNullException(nameof(member));
		    dbMember.Salaries.Add(salary);
		    await Server.Db.SaveChangesAsync();
		}
    }
}
