using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Infrastructure;
using System.Linq;
using System.Security.Principal;
using IgiCore.SDK.Core.Diagnostics;
using IgiCore.SDK.Server.Configuration;
using IgiCore.SDK.Server.Controllers;
using IgiCore.SDK.Server.Events;
using IgiCore.SDK.Server.Rpc;
using Roleplay.Server.Storage;

namespace Roleplay.Server.Controllers
{
	public class CharacterController : ConfigurableController<Configuration>
	{
		public CharacterController(ILogger logger, IEventManager events, IRpcHandler rpc, Configuration configuration) : base(logger, events, rpc, configuration)
		{
			this.Rpc.Event("characters:list").On(Characters);

			InitializeDatabase();
		}

		public void InitializeDatabase()
		{
			var migrator = new DbMigrator(new Migrations.Configuration());
			migrator.Configuration.TargetDatabase = new DbConnectionInfo(ServerConfiguration.DatabaseConnection, "MySql.Data.MySqlClient");
			this.Logger.Debug("Checking for migrations...");
			if (migrator.GetPendingMigrations().Any())
			{
				this.Logger.Debug($"Migrations count: {migrator.GetPendingMigrations().Count()}");
				migrator.Update();
			}
			this.Logger.Debug("Migrations ran");
		}

		public void Characters(IRpcEvent e)
		{
			this.Logger.Info($"Characters: {e.User.Name}");

			using (var context = new CharacterContext())
			{
				var characters = context.Characters.Where(c => c.User.Id == e.User.Id && c.Deleted == null).OrderBy(c => c.Created).ToList();

				e.Reply(characters);
			}
		}
	}
}
