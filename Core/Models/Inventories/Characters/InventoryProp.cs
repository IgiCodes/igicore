using System;
using System.ComponentModel.DataAnnotations;
using IgiCore.Core.Extensions;
using IgiCore.Core.Models.Appearance;

namespace IgiCore.Core.Models.Inventories.Characters
{
    public class InventoryProp : IProp
    {
        [Key] public Guid Id { get; set; } = GuidGenerator.GenerateTimeBasedGuid();
        public Prop Value { get; set; }
    }
}
