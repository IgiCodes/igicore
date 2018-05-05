using System;
using IgiCore.Core.Extensions;
using IgiCore.Core.Models.Economy.Banking;

namespace IgiCore.Server.Models.Economy.Banking
{
	public class BankBranch : IBankBranch
	{
		public Guid Id { get; set; }
		public DateTime Created { get; set; }
		public DateTime? Deleted { get; set; }
		public string Name { get; set; }

	    public BankBranch()
	    {
	        this.Id = GuidGenerator.GenerateTimeBasedGuid();
            Created = DateTime.UtcNow;
	    }
    }
}
