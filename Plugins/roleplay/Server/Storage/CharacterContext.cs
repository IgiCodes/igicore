using System.Data.Entity;
using IgiCore.SDK.Server.Storage;
using Roleplay.Core.Models.Player;

namespace Roleplay.Server.Storage
{
	public class CharacterContext : EFContext<CharacterContext>
	{
		public DbSet<Character> Characters { get; set; }
	}
}
