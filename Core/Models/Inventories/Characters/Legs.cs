namespace IgiCore.Core.Models.Inventories.Characters
{
    public class Legs : InventoryComponent
    {
        public Leg Left { get; set; } = new Leg();
        public Leg Right { get; set; } = new Leg();
        public Feet Feet { get; set; } = new Feet();
    }
}
