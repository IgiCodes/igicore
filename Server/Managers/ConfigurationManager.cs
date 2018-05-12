using System;
using System.IO;
using CitizenFX.Core.Native;
using IgiCore.Server.Models.Settings;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace IgiCore.Server.Managers
{
	public static class ConfigurationManager
	{
		public static string ConfigurationFile { get; }

		public static string HostName => API.GetConvar("sv_hostname", "dev");
		public static int MaxClients => API.GetConvarInt("sv_maxclients", 32);
		public static string LicenseKey => API.GetConvar("sv_licensekey", string.Empty);

		public static Configuration Configuration { get; }

		static ConfigurationManager()
		{
			try
			{
				ConfigurationFile = API.GetConvar("igicore_config", string.Empty);
			}
			catch (Exception)
			{
				// ignored
			}

			if (string.IsNullOrWhiteSpace(ConfigurationFile) || !File.Exists(ConfigurationFile))
			{
				ConfigurationFile = ResolveCurrentPath("igicore.yml");
			}

			Deserializer deserializer = new DeserializerBuilder()
				.WithNamingConvention(new PascalCaseNamingConvention())
				.IgnoreUnmatchedProperties()
				.Build();

			Configuration = deserializer.Deserialize<Configuration>(File.ReadAllText(ConfigurationFile));
		}

		public static string ResolveCurrentPath(string searchFile)
		{
			var basePath = AppDomain.CurrentDomain.BaseDirectory;
			var resourcePath = "igicore";

			try
			{
				resourcePath = API.GetCurrentResourceName();
			}
			catch (Exception)
			{
				// ignored
			}

			foreach (var path in new[] {
				Path.Combine(basePath),
				Path.Combine(basePath, resourcePath),
				Path.Combine(basePath, "igicore", resourcePath),
				Path.Combine(basePath, "[igicore]", resourcePath),
				Path.Combine(basePath, "[igicore]", resourcePath),
				Path.Combine(basePath, "[igicore]", "igicore", resourcePath),
				Path.Combine(basePath, "resources", resourcePath),
				Path.Combine(basePath, "resources", "igicore", resourcePath),
				Path.Combine(basePath, "resources", "[igicore]", resourcePath),
				Path.Combine(basePath, "resources", "[igicore]", resourcePath),
				Path.Combine(basePath, "resources", "[igicore]", "igicore", resourcePath)
			})
			{
				if (Directory.Exists(path) && File.Exists(Path.Combine(path, searchFile))) return Path.Combine(path, searchFile);
			}

			throw new Exception("Cannot locate current directory" + basePath);
		}
	}
}
