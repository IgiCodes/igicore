using System;
using System.ComponentModel.DataAnnotations;
using IgiCore.Core.Extensions;
using IgiCore.Models.Appearance;

namespace IgiCore.Core.Models.Inventories.Characters
{
    public class InventoryComponent : IComponent
    {
        [Key] public Guid Id { get; set; } = GuidGenerator.GenerateTimeBasedGuid();
        public Component Component { get; set; } = new Component();
    }
}
