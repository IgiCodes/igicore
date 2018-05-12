using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using CitizenFX.Core;
using IgiCore.Core.Models.Economy.Banking;
using IgiCore.Server.Models.Economy.Banking;
using IgiCore.Server.Storage.MySql;

namespace IgiCore.Server.Migrations
{
	internal sealed class Configuration : DbMigrationsConfiguration<DB>
	{
		public Configuration()
		{
			this.TargetDatabase = new DbConnectionInfo(Config.MySqlConnString, "MySql.Data.MySqlClient");
			this.AutomaticMigrationsEnabled = true;
		}

    }
}
