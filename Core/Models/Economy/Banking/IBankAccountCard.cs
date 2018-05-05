using System;
using IgiCore.Core.Models.Objects.Items.Economy;

namespace IgiCore.Core.Models.Economy.Banking
{
	public interface IBankAccountCard
	{
		Guid Id { get; set; }
		DateTime Created { get; set; }
		DateTime? Deleted { get; set; }
		BankCard Card { get; set; }
		int Pin { get; set; }
		int Number { get; set; }
		string Name { get; set; }
	}
}
