using System.Data.Entity;
using IgiCore.Core.Models.Appearance;
using IgiCore.Core.Models.Inventories.Characters;
using IgiCore.Core.Models.Objects.Vehicles;
using IgiCore.Server.Migrations;
using IgiCore.Server.Models;
using IgiCore.Server.Models.Economy.Banking;
using IgiCore.Server.Models.Groups;
using IgiCore.Server.Models.Player;
using MySql.Data.Entity;

namespace IgiCore.Server.Storage.MySql
{
	[DbConfigurationType(typeof(MySqlEFConfiguration))]
	public class DB : DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<Character> Characters { get; set; }

		public DbSet<Group> Groups { get; set; }
		public DbSet<GroupMember> GroupMembers { get; set; }
		public DbSet<GroupRole> GroupRoles { get; set; }

		public DbSet<Style> Styles { get; set; }

		public DbSet<Vehicle> Vehicles { get; set; }
		public DbSet<Car> Cars { get; set; }
		public DbSet<Bike> Bikes { get; set; }

		public DbSet<Inventory> Inventories { get; set; }

		public DbSet<Bank> Banks { get; set; }
		public DbSet<BankBranch> BankBranches { get; set; }
		public DbSet<BankATM> BankATMs { get; set; }
		public DbSet<BankAccount> BankAccounts { get; set; }
		public DbSet<BankAccountCard> BankAccountCards { get; set; }
		public DbSet<BankAccountMember> BankAccountMembers { get; set; }

		public DB() : base(Config.MySqlConnString)
		{
			Database.SetInitializer(new MigrateDatabaseToLatestVersion<DB, Configuration>());

			//this.Database.Log = m => Server.Log(m);
		}
	}
}
