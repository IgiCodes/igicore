using System;
using System.ComponentModel.DataAnnotations;
using IgiCore.Core.Extensions;
using IgiCore.Core.Models.Objects.Items;

namespace IgiCore.Core.Models.Inventories.Characters
{
    public class Holster
    {
        [Key] public Guid Id { get; set; } = GuidGenerator.GenerateTimeBasedGuid();
        public StorableItem Item { get; set; }
        public int Size { get; set; }
    }
}
