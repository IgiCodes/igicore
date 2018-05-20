using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;

namespace IgiCore.Models.Groups
{
	[PublicAPI]
	public class Group : IdentityModel
	{
		[Required]
		[StringLength(200, MinimumLength = 2)] // TODO
		public string Name { get; set; }

		[InverseProperty("Group")]
		public virtual List<GroupRole> Roles { get; set; }

		[InverseProperty("Group")]
		public virtual List<GroupMember> Members { get; set; }
	}
}
