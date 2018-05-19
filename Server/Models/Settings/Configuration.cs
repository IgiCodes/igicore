namespace IgiCore.Server.Models.Settings
{
	public class Configuration
	{
		public string ServerName { get; protected set; } = "igicore";
		public string GameType { get; protected set; } = "Roleplay";
		public string MapName { get; protected set; } = "Los Santos";

		public DatabaseConnection Database { get; protected set; } = new DatabaseConnection();
	}
}
