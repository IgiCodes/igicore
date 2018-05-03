using System;
using System.Collections.Generic;
using IgiCore.Core.Models.Inventories.Characters;

namespace IgiCore.Core.Models.Player
{
	public interface ICharacter
	{
		Guid Id { get; set; }
		string Forename { get; set; }
		string Middlename { get; set; }
		string Surname { get; set; }
		DateTime DateOfBirth { get; set; }
		short Gender { get; set; }
		bool Alive { get; set; }
		int Health { get; set; }
		int Armor { get; set; }
		string Ssn { get; set; }
		float PosX { get; set; }
		float PosY { get; set; }
		float PosZ { get; set; }
		string Model { get; set; }
		string WalkingStyle { get; set; }
		Inventory Inventory { get; set; }
		DateTime LastPlayed { get; set; }
		DateTime? Deleted { get; set; }
		DateTime Created { get; set; }
		List<Skill> Skills { get; set; }
	}
}
