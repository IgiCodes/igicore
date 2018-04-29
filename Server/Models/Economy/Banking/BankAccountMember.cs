using System;
using IgiCore.Core.Models.Economy.Banking;
using IgiCore.Server.Models.Player;

namespace IgiCore.Server.Models.Economy.Banking
{
    public class BankAccountMember : IBankAccountMembers
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Deleted { get; set; }

        public Character Member { get; set; }
    }
}
