using System;

namespace IgiCore.Core.Models.Economy.Banking
{
	public interface IBankAccount
	{
		Guid Id { get; set; }
		DateTime Created { get; set; }
		DateTime? Deleted { get; set; }
		int AccountNumber { get; set; }
		BankAccountTypes Type { get; set; }
		double Balance { get; set; }
		bool Locked { get; set; }
	}
}
