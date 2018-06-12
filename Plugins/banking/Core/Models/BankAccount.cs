using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IgiCore.SDK.Core.Models;
using JetBrains.Annotations;

namespace Banking.Core.Models
{
	[PublicAPI]
	public class BankAccount : IdentityModel
	{
		[Required]
		[StringLength(200, MinimumLength = 2)] // TODO
		public string Name { get; set; }

		[Required]
		[StringLength(15, MinimumLength = 15)] // TODO
		public string AccountNumber { get; set; }

		[Required]
		[Range(0, 1)]
		public BankAccountTypes Type { get; set; }

		[Required]
		public double Balance { get; set; }

		[Required]
		public bool Locked { get; set; }

		[Required]
		[ForeignKey("Bank")]
		public Guid BankId { get; set; }

		public virtual Bank Bank { get; set; }

		[InverseProperty("Account")]
		public virtual List<BankAccountMember> Members { get; set; }

		[InverseProperty("Account")]
		public virtual List<BankAccountCard> Cards { get; set; }

		public BankAccount()
		{
			// TODO: Generate account number
		}
	}
}
