using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;

namespace Roleplay.Core.Models.Player.Skills
{
	[PublicAPI]
	public class Skill
	{
		[Key]
		[Required]
		public Guid Id { get; set; }

		[Required]
		[Range(0, 1)]
		public SkillType Type { get; set; }

		[Required]
		[StringLength(20, MinimumLength = 1)] // TODO
		public string Name { get; set; }

		[Required]
		[Range(0, 100)] // TODO
		public float Value { get; set; }

		[Required]
		[ForeignKey("Character")]
		public Guid CharacterId { get; set; }

		public virtual Character Character { get; set; }
	}
}
