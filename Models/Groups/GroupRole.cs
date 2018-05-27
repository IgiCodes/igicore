using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;

namespace IgiCore.Models.Groups
{
	[PublicAPI]
	public class GroupRole : IdentityModel
	{
		[Required]
		[StringLength(200, MinimumLength = 2)] // TODO
		public string Name { get; set; }

		[Required]
		[Range(0, 100)] // TODO
		public int Rank { get; set; }

		[Required]
		[ForeignKey("Group")]
		public Guid GroupId { get; set; }

		public virtual Group Group { get; set; }

		// TODO
		public virtual List<GroupMember> Members { get; set; }
	}
}
