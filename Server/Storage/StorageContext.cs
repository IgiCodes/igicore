using System.Data.Entity;
using IgiCore.SDK.Core.Models.Player;
using IgiCore.SDK.Server.Storage;

namespace IgiCore.Server.Storage
{
	public class StorageContext : EFContext<StorageContext>
	{
		public DbSet<Session> Sessions { get; set; }

		public DbSet<User> Users { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<User>().HasIndex(u => u.SteamId).IsUnique();
		}
	}
}
