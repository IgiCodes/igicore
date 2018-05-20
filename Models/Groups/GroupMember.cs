using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IgiCore.Models.Player;
using JetBrains.Annotations;

namespace IgiCore.Models.Groups
{
	[PublicAPI]
	public class GroupMember : IdentityModel
    {
	    [Required]
	    [ForeignKey("Character")]
	    public Guid CharacterId { get; set; }

	    public virtual Character Character { get; set; }

		[Required]
	    [ForeignKey("Group")]
	    public Guid GroupId { get; set; }

	    public virtual Group Group { get; set; }

		// TODO
		public virtual List<GroupRole> Roles { get; set; }

		// TODO
        //public virtual List<Salary> Salaries { get; set; }
	}
}
