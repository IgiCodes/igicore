// ReSharper disable All
namespace Roleplay.Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Characters",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Forename = c.String(nullable: false, maxLength: 100, unicode: false),
                        Middlename = c.String(maxLength: 100, unicode: false),
                        Surname = c.String(nullable: false, maxLength: 100, unicode: false),
                        DateOfBirth = c.DateTime(nullable: false, precision: 0),
                        Gender = c.Short(nullable: false),
                        Alive = c.Boolean(nullable: false),
                        Health = c.Int(nullable: false),
                        Armor = c.Int(nullable: false),
                        Ssn = c.String(nullable: false, maxLength: 9, unicode: false),
                        Position_X = c.Single(nullable: false),
                        Position_Y = c.Single(nullable: false),
                        Position_Z = c.Single(nullable: false),
                        Model = c.String(nullable: false, maxLength: 200, unicode: false),
                        WalkingStyle = c.String(nullable: false, maxLength: 200, unicode: false),
                        LastPlayed = c.DateTime(precision: 0),
                        UserId = c.Guid(nullable: false),
                        StyleId = c.Guid(nullable: false),
                        Created = c.DateTime(nullable: false, precision: 0),
                        Deleted = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Styles", t => t.StyleId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.StyleId);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Characters", "UserId", "dbo.Users");
            DropForeignKey("dbo.Characters", "StyleId", "dbo.Styles");

            DropIndex("dbo.Characters", new[] { "StyleId" });
            DropIndex("dbo.Characters", new[] { "UserId" });

            DropTable("dbo.Characters");
        }
    }
}
