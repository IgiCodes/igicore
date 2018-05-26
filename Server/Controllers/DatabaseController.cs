using System.Data.Entity;
using IgiCore.SDK.Core;
using IgiCore.SDK.Core.Diagnostics;
using IgiCore.SDK.Server;
using IgiCore.SDK.Server.Configuration;
using IgiCore.SDK.Server.Rpc;
using IgiCore.Server.Storage;
using MySql.Data.EntityFramework;

namespace IgiCore.Server.Controllers
{
	public class DatabaseController : ConfigurableController<DatabaseConfiguration>
	{
		public DatabaseController(ILogger logger, IEventsManager events, DatabaseConfiguration configuration) : base(logger, events, configuration)
		{
			// Set global database connection string
			ServerConfiguration.DatabaseConnection = this.Configuration.ToString();

			//MySqlTrace.Switch.Level = SourceLevels.All;
			//MySqlTrace.Listeners.Add(new ConsoleTraceListener());

			DbConfiguration.SetConfiguration(new MySqlEFConfiguration());

			using (var entities = new StorageContext())
			{
				if (entities.Database.Exists()) return;

				this.Logger.Info($"No existing database found, creating new database \"{this.Configuration.Database}\"");

				entities.Database.CreateIfNotExists();
			}
		}
	}

	public class DatabaseConfiguration : IControllerConfiguration
	{
		public string Host { get; set; } = "localhost";
		public int Port { get; set; } = 3306;
		public string Database { get; set; } = "fivem";
		public string User { get; set; } = "root";
		public string Password { get; set; } = string.Empty;
		public string Charset { get; set; } = "utf8mb4";

		public override string ToString() => $"Host={this.Host};Port={this.Port};Database={this.Database};User Id={this.User};Password={this.Password};CharSet={this.Charset};SSL Mode=None";
	}
}
