using System;

namespace Roleplay.Vehicles.Core.Models
{
    public interface IObject
    {
        Guid Id { get; set; }
        long Hash { get; set; }
        int? Handle { get; set; }
        Guid TrackingUserId { get; set; }
        int? NetId { get; set; }
        float PosX { get; set; }
        float PosY { get; set; }
        float PosZ { get; set; }
        float Heading { get; set; }
    }
}
