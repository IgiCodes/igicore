using System;
using System.ComponentModel.DataAnnotations.Schema;
using IgiCore.Core.Models.Player;

namespace IgiCore.Core.Models.Objects
{
    public interface IObject
    {
        Guid Id { get; set; }
        long Hash { get; set; }
        int? Handle { get; set; }
        Guid TrackingUserId { get; set; }
        int? NetId { get; set; }
        bool IsHoldable { get; set; }
        float PosX { get; set; }
        float PosY { get; set; }
        float PosZ { get; set; }
        float Heading { get; set; }

    }
}
