using System;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace IgiCore.Models
{
	[PublicAPI]
    public abstract class IdentityModel
    {
		[Key]
		[Required]
		public Guid Id { get; set; }

		[Required]
        public DateTime Created { get; set; }

	    public DateTime? Deleted { get; set; }

	    protected IdentityModel()
	    {
		    this.Created = DateTime.UtcNow;
	    }
	}
}
