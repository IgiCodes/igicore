namespace IgiCore.Server.Models.Settings
{
	public class DatabaseConnection
	{
		public string Host { get; protected set; } = "igicore";
		public int Port { get; protected set; } = 3306;
		public string Database { get; protected set; } = "fivem";
		public string User { get; protected set; } = "root";
		public string Password { get; protected set; } = "password";
		public string Charset { get; protected set; } = "utf8mb4";
		
		public override string ToString() => $"server={this.Host};port={this.Port};database={this.Database};user={this.User};password={this.Password};charset={this.Charset}";
	}
}
