namespace IgiCore.Core.Models.Inventories.Characters
{
    public class Arms : InventoryComponent
    {
        public virtual Arm Left { get; set; } = new Arm();
        public virtual Arm Right { get; set; } = new Arm();
        public virtual Hands Hands { get; set; } = new Hands();
        public virtual Wrists Wrists { get; set; } = new Wrists();
    }
}
