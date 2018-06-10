using System.Data.Entity;
using IgiCore.Models.Appearance;
using IgiCore.Models.Player;
using Roleplay.Core.Models.Player;
using IgiCore.SDK.Server.Storage.Contexts;
using MySql.Data.EntityFramework;
using IgiCore.Server.Diagnostics;

namespace Roleplay.Server.Storage
{
	[DbConfigurationType(typeof(MySqlEFConfiguration))]
	public class CharacterContext : EFContext
	{
		public DbSet<Character> Characters { get; set; }

		public CharacterContext()
		{
			//Database.SetInitializer(new MigrateDatabaseToLatestVersion<CharacterContext, Migrations.Configuration>());
			Database.SetInitializer<CharacterContext>(null);
			this.Database.Log = m => new Logger().Debug(m);
		}
	}
}
