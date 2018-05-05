using System;

namespace IgiCore.Core.Models.Economy.Banking
{
    public interface IBankATM
    {
        Guid Id { get; set; }
        DateTime Created { get; set; }
        DateTime? Deleted { get; set; }
        string Name { get; set; }
        uint Hash { get; set; }

        float PosX { get; set; }
        float PosY { get; set; }
        float PosZ { get; set; }
    }
}
