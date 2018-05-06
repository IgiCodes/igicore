using System;
using System.Collections.Generic;
using IgiCore.Core.Models.Economy.Banking;

namespace IgiCore.Core.Models.Connection
{
	public class ServerInformation
	{
		public string ResourceName { get; set; }
		public string ServerName { get; set; }
		public DateTime DateTime { get; set; }
		public string Weather { get; set; }
	    public List<BankAtm> Atms { get; set; }
	    public List<BankBranch> Branches { get; set; }
    } 
}
