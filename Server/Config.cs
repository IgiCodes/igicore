using CitizenFX.Core.Native;

namespace IgiCore.Server
{
	public static class Config
	{
		public static string HostName => API.GetConvar("sv_hostname", "dev");
		public static string ServerName => API.GetConvar("sv_servername", "dev");
		public static int MaxCLients => API.GetConvarInt("sv_maxclients", 31);
		public static string LicenseKey => API.GetConvar("sv_licensekey", string.Empty);
		public static string MySqlConnString => API.GetConvar("mysql_connection", string.Empty);
	}
}
