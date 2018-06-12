using System.Data.Entity;
using IgiCore.SDK.Server.Storage;
using Roleplay.Core.Models.Appearance;
using Roleplay.Core.Models.Groups;
using Roleplay.Core.Models.Player;

namespace Roleplay.Server.Storage
{
	public class CharacterContext : EFContext<CharacterContext>
	{
		public DbSet<Character> Characters { get; set; }

		public DbSet<Group> Groups { get; set; }

		public DbSet<GroupMember> GroupMembers { get; set; }

		public DbSet<GroupRole> GroupRoles { get; set; }

		public DbSet<Style> Styles { get; set; }
	}
}
