using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IgiCore.SDK.Core.Models;
using JetBrains.Annotations;

namespace Roleplay.Core.Models.Groups
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
