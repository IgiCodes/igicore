using System.Data.Entity;
using IgiCore.Core.Models.Appearance;
using IgiCore.Server.Migrations;
using IgiCore.Server.Models;
using MySql.Data.Entity;

namespace IgiCore.Server.Storage.MySql
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class DB : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Character> Characters { get; set; }

        public DB() : base(Config.MySqlConnString)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DB, Configuration>());
        }
    }
}
