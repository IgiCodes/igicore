using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IgiCore.Core.Extensions;

namespace IgiCore.Core.Models.Inventories.Characters
{
    [Table("arms_arms")]
    public class Arm
    {
        [Key] public Guid Id { get; set; } = GuidGenerator.GenerateTimeBasedGuid();
    }
}
