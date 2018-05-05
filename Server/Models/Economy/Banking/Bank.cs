using System;
using System.Collections.Generic;
using IgiCore.Core.Models.Economy.Banking;

namespace IgiCore.Server.Models.Economy.Banking
{
	public class Bank : IBank
	{
		public Guid Id { get; set; }
		public DateTime Created { get; set; }
		public DateTime? Deleted { get; set; }
		public string Name { get; set; }

		public virtual List<BankAccount> Accounts { get; set; }
		public virtual List<BankBranch> Branches { get; set; }
		public virtual List<BankATM> ATMs { get; set; }
	}
}
