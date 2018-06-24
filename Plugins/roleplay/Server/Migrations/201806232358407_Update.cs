namespace Roleplay.Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Characters", "Alive", c => c.Boolean(nullable: false, storeType: "bit"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Characters", "Alive", c => c.Boolean(nullable: false));
        }
    }
}
