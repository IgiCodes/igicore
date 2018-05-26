using System;
using System.IO;
using CitizenFX.Core.Native;
using IgiCore.Server.Plugins;

namespace IgiCore.Server.Configuration
{
	public static class FileManager
	{
		public static string ResolveResourcePath(string searchFile = PluginManager.DefinitionFile)
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
				if (Directory.Exists(path) && File.Exists(Path.Combine(path, searchFile))) return path;
			}

			throw new DirectoryNotFoundException($"Unable to locate resource directory \"{resourcePath}\" at base \"{basePath}\"");
		}
	}
}
