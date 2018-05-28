using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;

namespace IgiCore.Models.Player
{
	[PublicAPI]
	public class User : IdentityModel
	{
		[Required]
		public long SteamId { get; set; }

		[Required]
		[StringLength(32, MinimumLength = 1)] // TODO: Confirm
		public string Name { get; set; }

		public DateTime? AcceptedRules { get; set; }

		[InverseProperty("User")]
		public virtual List<Session> Sessions { get; set; }

		[InverseProperty("User")]
		public virtual List<Character> Characters { get; set; }
	}
}
