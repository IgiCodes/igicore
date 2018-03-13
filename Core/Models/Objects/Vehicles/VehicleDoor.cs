namespace IgiCore.Core.Models.Objects.Vehicles
{
    public class VehicleDoor
    {
        public VehicleDoorIndex Index { get; set; }
        public bool IsOpen { get; set; }
        public bool IsClosed => !this.IsOpen;
        public bool IsBroken { get; set; }
    }
}