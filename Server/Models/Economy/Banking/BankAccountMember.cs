using System;
using IgiCore.Core.Extensions;
using IgiCore.Core.Models.Economy.Banking;
using IgiCore.Core.Models.Player;
using IgiCore.Server.Models.Player;

namespace IgiCore.Server.Models.Economy.Banking
{
	public class BankAccountMember : IBankAccountMembers
	{
		public Guid Id { get; set; }
		public DateTime Created { get; set; }
		public DateTime? Deleted { get; set; }

		public virtual BankAccount Account { get; set; }
		public virtual Character Member { get; set; }

		public BankAccountMember()
		{
			this.Id = GuidGenerator.GenerateTimeBasedGuid();
			this.Created = DateTime.UtcNow;
		}
	}
}
