using System;
using System.ComponentModel.DataAnnotations;
using IgiCore.Core.Extensions;

namespace IgiCore.Core.Models.Inventories.Characters
{
    public class Inventory
    {
        [Key] public Guid Id { get; set; } = GuidGenerator.GenerateTimeBasedGuid();

        public Arms Arms { get; set; } = new Arms();
        public Head Head { get; set; } = new Head();
        public Legs Legs { get; set; } = new Legs();
        public Torso Torso { get; set; } = new Torso();
    }
}
