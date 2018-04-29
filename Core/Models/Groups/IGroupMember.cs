using System;

namespace IgiCore.Core.Models.Groups
{
    public interface IGroupMember
    {
        Guid Id { get; set; }
        DateTime Created { get; set; }
        DateTime? Deleted { get; set; }
    }
}
