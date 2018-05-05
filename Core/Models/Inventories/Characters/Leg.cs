using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IgiCore.Core.Extensions;

namespace IgiCore.Core.Models.Inventories.Characters
{
    [Table("legs_legs")]
    public class Leg
    {
        [Key] public Guid Id { get; set; } = GuidGenerator.GenerateTimeBasedGuid();
        public Holster LegHolster { get; set; }
        public Holster WaistHolster { get; set; }
        public virtual List<Pocket> Pockets { get; set; } = new List<Pocket>();
    }
}
