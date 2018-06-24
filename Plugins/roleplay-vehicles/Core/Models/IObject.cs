using System;
using IgiCore.SDK.Core.Models;

namespace Roleplay.Vehicles.Core.Models
{
    public interface IObject
    {
        Guid Id { get; set; }
        long Hash { get; set; }
        int? Handle { get; set; }
        Guid TrackingUserId { get; set; }
        int? NetId { get; set; }
        Position Position { get; set; }
        float Heading { get; set; }
    }
}
