using System;
using IgiCore.Core.Models.Economy.Banking;
using IgiCore.Core.Models.Objects.Items.Economy;

namespace IgiCore.Server.Models.Economy.Banking
{
	public class BankAccountCard : IBankAccountCard
	{
		public Guid Id { get; set; }
		public DateTime Created { get; set; }
		public DateTime? Deleted { get; set; }
		public BankCard Card { get; set; }
		public int Pin { get; set; }
		public int Number { get; set; }
		public string Name { get; set; }
	}
}
