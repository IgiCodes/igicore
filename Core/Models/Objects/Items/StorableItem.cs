using IgiCore.Core.Models.Inventories;

namespace IgiCore.Core.Models.Objects.Items
{
    public class StorableItem : Item, IStorable
    {
        public int Size { get; set; }
        public bool IsStored { get; set; }
        public Container Container { get; set; }
    }
}
