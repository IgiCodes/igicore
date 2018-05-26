using System.Data.Entity.Migrations;
using IgiCore.Server.Storage;

namespace IgiCore.Server.Migrations
{
	internal sealed class Configuration : DbMigrationsConfiguration<StorageContext>
	{
		public Configuration()
		{
			this.AutomaticMigrationsEnabled = true;
		}
    }
}
