namespace Banking.Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BankAccountCards",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        Number = c.String(nullable: false, maxLength: 12, storeType: "nvarchar"),
                        Pin = c.Int(nullable: false),
                        AccountId = c.Guid(nullable: false),
                        Created = c.DateTime(nullable: false, precision: 0),
                        Deleted = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BankAccounts", t => t.AccountId, cascadeDelete: true)
                .Index(t => t.AccountId);
            
            CreateTable(
                "dbo.BankAccounts",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        AccountNumber = c.String(nullable: false, maxLength: 15, storeType: "nvarchar"),
                        Type = c.Int(nullable: false),
                        Balance = c.Double(nullable: false),
                        Locked = c.Boolean(nullable: false),
                        BankId = c.Guid(nullable: false),
                        Created = c.DateTime(nullable: false, precision: 0),
                        Deleted = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Banks", t => t.BankId, cascadeDelete: true)
                .Index(t => t.BankId);
            
            CreateTable(
                "dbo.Banks",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        Created = c.DateTime(nullable: false, precision: 0),
                        Deleted = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BankAtms",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        Hash = c.Long(nullable: false),
                        Position_X = c.Single(nullable: false),
                        Position_Y = c.Single(nullable: false),
                        Position_Z = c.Single(nullable: false),
                        BankId = c.Guid(nullable: false),
                        Created = c.DateTime(nullable: false, precision: 0),
                        Deleted = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Banks", t => t.BankId, cascadeDelete: true)
                .Index(t => t.BankId);
            
            CreateTable(
                "dbo.BankBranches",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        Position_X = c.Single(nullable: false),
                        Position_Y = c.Single(nullable: false),
                        Position_Z = c.Single(nullable: false),
                        Heading = c.Single(nullable: false),
                        BankId = c.Guid(nullable: false),
                        Created = c.DateTime(nullable: false, precision: 0),
                        Deleted = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Banks", t => t.BankId, cascadeDelete: true)
                .Index(t => t.BankId);
            
            CreateTable(
                "dbo.BankAccountMembers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        MemberId = c.Guid(nullable: false),
                        AccountId = c.Guid(nullable: false),
                        Created = c.DateTime(nullable: false, precision: 0),
                        Deleted = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Characters", t => t.MemberId, cascadeDelete: true)
                .ForeignKey("dbo.BankAccounts", t => t.AccountId, cascadeDelete: true)
                .Index(t => t.MemberId)
                .Index(t => t.AccountId);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BankAccountMembers", "AccountId", "dbo.BankAccounts");
            DropForeignKey("dbo.BankAccountMembers", "MemberId", "dbo.Characters");
            DropForeignKey("dbo.BankAccountCards", "AccountId", "dbo.BankAccounts");
            DropForeignKey("dbo.BankBranches", "BankId", "dbo.Banks");
            DropForeignKey("dbo.BankAtms", "BankId", "dbo.Banks");
            DropForeignKey("dbo.BankAccounts", "BankId", "dbo.Banks");
            
            DropIndex("dbo.BankAccountMembers", new[] { "AccountId" });
            DropIndex("dbo.BankAccountMembers", new[] { "MemberId" });
            DropIndex("dbo.BankBranches", new[] { "BankId" });
            DropIndex("dbo.BankAtms", new[] { "BankId" });
            DropIndex("dbo.BankAccounts", new[] { "BankId" });
            DropIndex("dbo.BankAccountCards", new[] { "AccountId" });
           
            DropTable("dbo.BankAccountMembers");
            DropTable("dbo.BankBranches");
            DropTable("dbo.BankAtms");
            DropTable("dbo.Banks");
            DropTable("dbo.BankAccounts");
            DropTable("dbo.BankAccountCards");
        }
    }
}
