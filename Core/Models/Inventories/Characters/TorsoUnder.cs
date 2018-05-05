using System.Collections.Generic;

namespace IgiCore.Core.Models.Inventories.Characters
{
    public class TorsoUnder : InventoryComponent
    {
        public virtual List<Pocket> Pockets { get; set; }
    }
}
