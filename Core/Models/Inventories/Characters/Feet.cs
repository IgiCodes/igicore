namespace IgiCore.Core.Models.Inventories.Characters
{
    public class Feet : InventoryComponent
    {
        public Foot Left { get; set; } = new Foot();
        public Foot Right { get; set; } = new Foot();
    }
}
