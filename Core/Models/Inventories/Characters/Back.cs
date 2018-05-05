using System;
using System.ComponentModel.DataAnnotations;
using IgiCore.Core.Extensions;

namespace IgiCore.Core.Models.Inventories.Characters
{
    public class Back
    {
        [Key] public Guid Id { get; set; } = GuidGenerator.GenerateTimeBasedGuid();
        public Backpack Backpack { get; set; }
        public BackGun GunL { get; set; }
        public BackGun GunR { get; set; }
    }
}
