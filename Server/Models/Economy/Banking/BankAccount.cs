using System;
using System.Collections.Generic;
using IgiCore.Core.Extensions;
using IgiCore.Core.Models.Economy.Banking;

namespace IgiCore.Server.Models.Economy.Banking
{
	public class BankAccount : IBankAccount
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public int AccountNumber { get; set; }
		public BankAccountTypes Type { get; set; }
		public double Balance { get; set; }
		public bool Locked { get; set; }
		public DateTime Created { get; set; }
		public DateTime? Deleted { get; set; }

		public virtual List<BankAccountMember> Members { get; set; }
		public virtual List<BankAccountCard> Cards { get; set; }

		public BankAccount()
		{
			this.Id = GuidGenerator.GenerateTimeBasedGuid();
			this.Name = string.Empty;
			this.AccountNumber = 1; // TODO: Generator
			this.Type = BankAccountTypes.Personal;
			this.Balance = 0;
			this.Locked = false;
			this.Created = DateTime.UtcNow;
		}
	}
}
