using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IgiCore.SDK.Core.Models;
using JetBrains.Annotations;

namespace Banking.Core.Models
{
	[PublicAPI]
	public class BankAccountCard : IdentityModel
	{
		[Required]
		[StringLength(200, MinimumLength = 2)] // TODO
		public string Name { get; set; }

		[Required]
		[StringLength(12, MinimumLength = 12)] // TODO
		public string Number { get; set; }

		[Required]
		[Range(0, 9999)]
		public int Pin { get; set; }

		[Required]
		[ForeignKey("Account")]
		public Guid AccountId { get; set; }

		public virtual BankAccount Account { get; set; }
	}
}
