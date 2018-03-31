using System;
using System.ComponentModel.DataAnnotations;
using IgiCore.Core.Extensions;

namespace IgiCore.Core.Models.Inventories.Characters
{
    public class Wrists
    {
        [Key] public Guid Id { get; set; } = GuidGenerator.GenerateTimeBasedGuid();
        public InventoryProp Watch { get; set; }
        public InventoryProp Wristband { get; set; }
    }
}
