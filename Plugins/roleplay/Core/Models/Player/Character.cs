using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IgiCore.Models;
using IgiCore.Models.Appearance;
using IgiCore.Models.Player;
using IgiCore.Models.Player.Skills;
using JetBrains.Annotations;

namespace Roleplay.Core.Models.Player
{
	[PublicAPI]
	public class Character : IdentityModel
	{
		[Required]
		[StringLength(100, MinimumLength = 2)]
		public string Forename { get; set; }

		[StringLength(100, MinimumLength = 1)]
		public string Middlename { get; set; }

		[Required]
		[StringLength(100, MinimumLength = 2)]
		public string Surname { get; set; }

		[Required]
		public DateTime DateOfBirth { get; set; }

		[Required]
		[Range(0, 1)]
		public short Gender { get; set; }

		[Required]
		public bool Alive { get; set; }

		[Required]
		[Range(0, 100)]
		public int Health { get; set; }

		[Required]
		[Range(0, 100)]
		public int Armor { get; set; }

		[Required]
		[StringLength(9, MinimumLength = 9)]
		public string Ssn { get; set; }

		[Required]
		public Position Position { get; set; }

		[Required]
		[StringLength(200)] // TODO
		public string Model { get; set; }

		[Required]
		[StringLength(200)] // TODO
		public string WalkingStyle { get; set; }

		public DateTime? LastPlayed { get; set; }

		[Required]
		[ForeignKey("User")]
		public Guid UserId { get; set; }

		public virtual User User { get; set; }

		//[InverseProperty("Character")]
		//public virtual List<Skill> Skills { get; set; }

		[Required]
		[ForeignKey("Style")]
		public Guid StyleId { get; set; }

		public virtual Style Style { get; set; }

		public string FullName => $"{this.Forename} {this.Middlename} {this.Surname}".Replace("  ", " ");

		public Character()
		{
			// TODO: Generate SSN
		}
	}
}
