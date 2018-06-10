using System.Data.Entity;
using IgiCore.Models.Appearance;
using IgiCore.Models.Groups;
using IgiCore.Models.Player;
using IgiCore.SDK.Server.Storage;

namespace IgiCore.Server.Storage
{
	public class StorageContext : EFContext<StorageContext>
	{
		public DbSet<Session> Sessions { get; set; }

		public DbSet<User> Users { get; set; }

		public DbSet<Group> Groups { get; set; }

		public DbSet<GroupMember> GroupMembers { get; set; }

		public DbSet<GroupRole> GroupRoles { get; set; }

		public DbSet<Style> Styles { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<User>().HasIndex(u => u.SteamId).IsUnique();
		}
	}
}
