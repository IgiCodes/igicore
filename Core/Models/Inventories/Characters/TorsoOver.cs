using System.Collections.Generic;

namespace IgiCore.Core.Models.Inventories.Characters
{
    public class TorsoOver : InventoryComponent
    {
        public virtual List<Pocket> Pockets { get; set; }
    }
}
