using System;
using System.ComponentModel.DataAnnotations;
using IgiCore.Core.Extensions;

namespace IgiCore.Core.Models.Inventories.Characters
{
    public class Head
    {
        [Key] public Guid Id { get; set; } = GuidGenerator.GenerateTimeBasedGuid();
        public InventoryProp Hat { get; set; }
        public InventoryProp Glasses { get; set; }
        public InventoryProp Earrings { get; set; }
    }
}
