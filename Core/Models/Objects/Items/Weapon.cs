using System;

namespace IgiCore.Core.Models.Objects.Items
{
    public class Weapon : IWeapon
    {
        public Guid Id { get; set; }
        public long Hash { get; set; }
        public int? Handle { get; set; }
        public Guid TrackingUserId { get; set; }
        public int? NetId { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }
        public float Heading { get; set; }
    }
}
