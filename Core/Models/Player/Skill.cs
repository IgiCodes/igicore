using System;

namespace IgiCore.Core.Models.Player
{
    public class Skill
    {
        public Guid Id { get; set; }
        public SkillType Type { get; set; }
        public string Name { get; set; }
        public float Value { get; set; }
    }
}
