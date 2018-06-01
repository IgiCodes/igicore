using IgiCore.SDK.Core.Diagnostics;

namespace IgiCore.SDK.Server.Configuration
{
	public static class ServerConfiguration
	{
		public static LogLevel LogLevel { get; set; } = LogLevel.Debug;

		public static string DatabaseConnection { get; set; } = "Host=localhost";
	}
}
