using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgiCore.SDK.Server.Configuration
{
	public static class ServerConfiguration
	{
		public static string DatabaseConnection { get; set; }

		public static void Load(IConfiguration configuration)
		{
			DatabaseConnection = configuration.DatabaseConnection;
		}
	}
}
