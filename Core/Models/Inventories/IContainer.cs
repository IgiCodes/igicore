using System;
using System.Collections.Generic;
using IgiCore.Core.Models.Objects.Items;

namespace IgiCore.Core.Models.Inventories
{
    public interface IContainer
    {
        Guid Id { get; set; }
        List<StorableItem> Storage { get; set; }
        int Size { get; set; }
    }
}
