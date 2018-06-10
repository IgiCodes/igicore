using IgiCore.SDK.Core.Controllers;

namespace IgiCore.Server.Configuration
{
	public class DatabaseConfiguration : IControllerConfiguration
	{
		public string Host { get; set; } = "localhost";

		public int Port { get; set; } = 3306;

		public string Database { get; set; } = "fivem";

		public string User { get; set; } = "root";

		public string Password { get; set; } = string.Empty;

		public string Charset { get; set; } = "utf8mb4";

		public override string ToString() => $"Host={this.Host};Port={this.Port};Database={this.Database};User Id={this.User};Password={this.Password};CharSet={this.Charset};SSL Mode=None;"; // ;Logging=true";
	}
}
