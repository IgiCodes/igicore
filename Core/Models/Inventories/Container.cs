using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IgiCore.Core.Extensions;
using IgiCore.Core.Models.Objects.Items;

namespace IgiCore.Core.Models.Inventories
{
    [Table("inv_containers")]
    public class Container : IContainer
    {
        [Key] public Guid Id { get; set; } = GuidGenerator.GenerateTimeBasedGuid();
        public virtual List<StorableItem> Storage { get; set; } = new List<StorableItem>();
        public int Size { get; set; }
    }
}
