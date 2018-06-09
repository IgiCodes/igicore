using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IgiCore.Models;
using JetBrains.Annotations;

namespace Banking.Core.Models
{
	[PublicAPI]
	public class Bank : IdentityModel
	{
		[Required]
		[StringLength(200, MinimumLength = 2)] // TODO
		public string Name { get; set; }

		[InverseProperty("Bank")]
		public virtual List<BankBranch> Branches { get; set; }

		[InverseProperty("Bank")]
		public virtual List<BankAtm> Atms { get; set; }

		[InverseProperty("Bank")]
		public virtual List<BankAccount> Accounts { get; set; }
	}
}
