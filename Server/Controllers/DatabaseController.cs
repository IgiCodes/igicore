using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using IgiCore.SDK.Core.Diagnostics;
using IgiCore.SDK.Server.Configuration;
using IgiCore.SDK.Server.Controllers;
using IgiCore.SDK.Server.Events;
using IgiCore.SDK.Server.Rpc;
using IgiCore.Server.Configuration;
using IgiCore.Server.Storage;
using MySql.Data.EntityFramework;
using MySql.Data.MySqlClient;

namespace IgiCore.Server.Controllers
{
	public class DatabaseController : ConfigurableController<DatabaseConfiguration>
	{
		public DatabaseController(ILogger logger, IEventManager events, IRpcHandler rpc, DatabaseConfiguration configuration) : base(logger, events, rpc, configuration)
		{
			// Set global database connection string
			ServerConfiguration.DatabaseConnection = this.Configuration.ToString();

			// Use MySQL EF adapter
			DbConfiguration.SetConfiguration(new MySqlEFConfiguration());

			// Enable SQL query logging
			MySqlTrace.Switch.Level = SourceLevels.All;
			MySqlTrace.Listeners.Add(new ConsoleTraceListener());

			using (var context = new StorageContext())
			{
				// Create database if needed
				if (!context.Database.Exists())
				{
					this.Logger.Info($"No existing database found, creating new database \"{this.Configuration.Database}\"");

					context.Database.CreateIfNotExists();
				}

				// Prime the connection cache
				// ReSharper disable once ReturnValueOfPureMethodIsNotUsed
				context.Users.FirstOrDefault();
			}
		}
	}
}
