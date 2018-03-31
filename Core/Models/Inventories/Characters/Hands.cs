namespace IgiCore.Core.Models.Inventories.Characters
{
    public class Hands : InventoryComponent
    {
        public Hand Right { get; set; } = new Hand();
        public Hand Left { get; set; } = new Hand();
    }
}
