using System;
using IgiCore.Core.Extensions;
using IgiCore.Core.Models.Objects.Items.Economy;

namespace IgiCore.Core.Models.Economy.Banking
{
	public class BankAccountCard : IBankAccountCard
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public int Number { get; set; }
		public int Pin { get; set; }
		public BankCard Card { get; set; }
		public DateTime Created { get; set; }
		public DateTime? Deleted { get; set; }

		public BankAccountCard()
		{
			this.Id = GuidGenerator.GenerateTimeBasedGuid();
			this.Name = string.Empty;
			this.Number = 1; // TODO: Generator
			this.Pin = 0000;
			this.Created = DateTime.UtcNow;
		}
	}
}
