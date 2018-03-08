using MySql.Data.Entity;
using System.Data.Common;
using System.Data.Entity;
using System.Configuration;
using MySql.Data.MySqlClient;
using IgiCore;
using CitizenFX.Core;
using IgiCore.Server.Models;

namespace IgiCore.Server.Storage.MySql
{
    // Code-Based Configuration and Dependency resolution
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class DB : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Character> Characters { get; set; }

        public DB() : base(Config.MySqlConnString)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DB, Migrations.Configuration>());
        }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<User>()
        //        .Map(m =>
        //        {
        //            m.MapInheritedProperties();
        //            m.ToTable("users");
        //        })
        //        .HasMany<Character>(u => u.Characters)
        //        .WithRequired(c => c.User)
        //        .HasForeignKey<int>(c => c.UserId);

        //    modelBuilder.Entity<Character>()
        //        .Map(m =>
        //        {
        //            m.MapInheritedProperties();
        //            m.ToTable("characters");
        //        })
        //        .HasRequired<User>(c => c.User)
        //        .WithMany(u => u.Characters)
        //        .HasForeignKey<int>(c => c.UserId);
        //}

    }
}