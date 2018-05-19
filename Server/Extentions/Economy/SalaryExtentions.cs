using System;
using System.Linq;
using System.Threading.Tasks;
using IgiCore.Core.Models.Economy;
using IgiCore.Core.Models.Groups;

namespace IgiCore.Server.Extentions.Economy
{
	public static class SalaryExtentions
	{
		public static async Task Delete(this Salary salary)
		{
			GroupMember dbGroupMember = Server.Db.GroupMembers.NotDeleted().FirstOrDefault(g => g.Id == salary.Member.Id);
			if (dbGroupMember == null) throw new ArgumentNullException(nameof(salary));
			dbGroupMember.Salaries.Remove(salary);
			await Server.Db.SaveChangesAsync();
			await salary.SoftDelete();
		}
	}
}
