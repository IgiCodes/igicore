using System;
using System.ComponentModel.DataAnnotations;
using IgiCore.Core.Extensions;

namespace IgiCore.Core.Models.Inventories.Characters
{
    public class Torso
    {
        [Key] public Guid Id { get; set; } = GuidGenerator.GenerateTimeBasedGuid();
        public Back Back { get; set; } = new Back();
        public TorsoOver Over { get; set; }
        public TorsoUnder Under { get; set; }
        public Holster LeftHolster { get; set; }
        public Holster RightHolster { get; set; }
    }
}
