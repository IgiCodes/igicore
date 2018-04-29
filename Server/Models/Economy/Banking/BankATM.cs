using System;
using IgiCore.Core.Models.Economy.Banking;

namespace IgiCore.Server.Models.Economy.Banking
{
    public class BankATM : IBankATM
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Deleted { get; set; }
        public string Name { get; set; }
    }
}
