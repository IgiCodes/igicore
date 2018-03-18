using System.ComponentModel.DataAnnotations.Schema;

namespace IgiCore.Core.Models.Objects
{
    public interface IObject
    {
        uint Hash { get; set; }
        [NotMapped]
        int Handle { get; set; }
        bool IsHoldable { get; set; }
        float PosX { get; set; }
        float PosY { get; set; }
        float PosZ { get; set; }

    }
}
