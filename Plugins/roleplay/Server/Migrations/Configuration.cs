using System;
using System.Data.Entity.Migrations;
using Roleplay.Server.Storage;

namespace Roleplay.Server.Migrations
{
	internal sealed class Configuration : DbMigrationsConfiguration<CharacterContext>
	{
		public Configuration()
		{
			//this.AutomaticMigrationsEnabled = true;
			AutomaticMigrationsEnabled = false;
		}
	}
}
