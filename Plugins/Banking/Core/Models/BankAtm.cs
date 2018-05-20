using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IgiCore.Models;
using JetBrains.Annotations;

namespace Banking.Core.Models
{
	[PublicAPI]
	public class BankAtm : IdentityModel
	{
		[Required]
		[StringLength(200, MinimumLength = 2)] // TODO
		public string Name { get; set; }

		[Required]
		public long Hash { get; set; }

		[Required]
		public Position Position { get; set; }

		[Required]
		[ForeignKey("Bank")]
		public Guid BankId { get; set; }

		public virtual Bank Bank { get; set; }
	}
}
