namespace IgiCore.Core.Models.Objects.Vehicles
{
    public enum VehicleLockStatus
    {
        None = 0,
        Unlocked = 1,
        Locked = 2,
        LockedForPlayer = 3,
        StickPlayerInside = 4,
        CanBeBrokenInto = 7,
        CanBeBrokenIntoPersist = 8,
        CannotBeTriedToEnter = 10
    }
}
