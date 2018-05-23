namespace IgiCore.Server.Migrations
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
            
            CreateTable(
                "dbo.Skills",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Type = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 20, unicode: false),
                        Value = c.Single(nullable: false),
                        CharacterId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Characters", t => t.CharacterId, cascadeDelete: true)
                .Index(t => t.CharacterId);
            
            CreateTable(
                "dbo.Styles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Face_Type = c.Int(nullable: false),
                        Face_Index = c.Int(nullable: false),
                        Face_Texture = c.Int(nullable: false),
                        Head_Type = c.Int(nullable: false),
                        Head_Index = c.Int(nullable: false),
                        Head_Texture = c.Int(nullable: false),
                        Hair_Type = c.Int(nullable: false),
                        Hair_Index = c.Int(nullable: false),
                        Hair_Texture = c.Int(nullable: false),
                        Torso_Type = c.Int(nullable: false),
                        Torso_Index = c.Int(nullable: false),
                        Torso_Texture = c.Int(nullable: false),
                        Torso2_Type = c.Int(nullable: false),
                        Torso2_Index = c.Int(nullable: false),
                        Torso2_Texture = c.Int(nullable: false),
                        Legs_Type = c.Int(nullable: false),
                        Legs_Index = c.Int(nullable: false),
                        Legs_Texture = c.Int(nullable: false),
                        Hands_Type = c.Int(nullable: false),
                        Hands_Index = c.Int(nullable: false),
                        Hands_Texture = c.Int(nullable: false),
                        Shoes_Type = c.Int(nullable: false),
                        Shoes_Index = c.Int(nullable: false),
                        Shoes_Texture = c.Int(nullable: false),
                        Special1_Type = c.Int(nullable: false),
                        Special1_Index = c.Int(nullable: false),
                        Special1_Texture = c.Int(nullable: false),
                        Special2_Type = c.Int(nullable: false),
                        Special2_Index = c.Int(nullable: false),
                        Special2_Texture = c.Int(nullable: false),
                        Special3_Type = c.Int(nullable: false),
                        Special3_Index = c.Int(nullable: false),
                        Special3_Texture = c.Int(nullable: false),
                        Textures_Type = c.Int(nullable: false),
                        Textures_Index = c.Int(nullable: false),
                        Textures_Texture = c.Int(nullable: false),
                        Hat_Type = c.Int(nullable: false),
                        Hat_Index = c.Int(nullable: false),
                        Hat_Texture = c.Int(nullable: false),
                        Glasses_Type = c.Int(nullable: false),
                        Glasses_Index = c.Int(nullable: false),
                        Glasses_Texture = c.Int(nullable: false),
                        EarPiece_Type = c.Int(nullable: false),
                        EarPiece_Index = c.Int(nullable: false),
                        EarPiece_Texture = c.Int(nullable: false),
                        Unknown3_Type = c.Int(nullable: false),
                        Unknown3_Index = c.Int(nullable: false),
                        Unknown3_Texture = c.Int(nullable: false),
                        Unknown4_Type = c.Int(nullable: false),
                        Unknown4_Index = c.Int(nullable: false),
                        Unknown4_Texture = c.Int(nullable: false),
                        Unknown5_Type = c.Int(nullable: false),
                        Unknown5_Index = c.Int(nullable: false),
                        Unknown5_Texture = c.Int(nullable: false),
                        Watch_Type = c.Int(nullable: false),
                        Watch_Index = c.Int(nullable: false),
                        Watch_Texture = c.Int(nullable: false),
                        Wristband_Type = c.Int(nullable: false),
                        Wristband_Index = c.Int(nullable: false),
                        Wristband_Texture = c.Int(nullable: false),
                        Unknown8_Type = c.Int(nullable: false),
                        Unknown8_Index = c.Int(nullable: false),
                        Unknown8_Texture = c.Int(nullable: false),
                        Unknown9_Type = c.Int(nullable: false),
                        Unknown9_Index = c.Int(nullable: false),
                        Unknown9_Texture = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        SteamId = c.String(nullable: false, maxLength: 15, unicode: false),
                        Name = c.String(nullable: false, maxLength: 32, unicode: false),
                        AcceptedRules = c.DateTime(precision: 0),
                        Created = c.DateTime(nullable: false, precision: 0),
                        Deleted = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Sessions",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        IpAddress = c.String(nullable: false, maxLength: 15, unicode: false),
                        Connected = c.DateTime(nullable: false, precision: 0),
                        Disconnected = c.DateTime(precision: 0),
                        DisconnectReason = c.String(maxLength: 200, unicode: false),
                        UserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.GroupMembers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CharacterId = c.Guid(nullable: false),
                        GroupId = c.Guid(nullable: false),
                        Created = c.DateTime(nullable: false, precision: 0),
                        Deleted = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Characters", t => t.CharacterId, cascadeDelete: true)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.CharacterId)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 200, unicode: false),
                        Created = c.DateTime(nullable: false, precision: 0),
                        Deleted = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GroupRoles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 200, unicode: false),
                        Rank = c.Int(nullable: false),
                        GroupId = c.Guid(nullable: false),
                        Created = c.DateTime(nullable: false, precision: 0),
                        Deleted = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.GroupRoleGroupMembers",
                c => new
                    {
                        GroupRole_Id = c.Guid(nullable: false),
                        GroupMember_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.GroupRole_Id, t.GroupMember_Id })
                .ForeignKey("dbo.GroupRoles", t => t.GroupRole_Id, cascadeDelete: true)
                .ForeignKey("dbo.GroupMembers", t => t.GroupMember_Id, cascadeDelete: true)
                .Index(t => t.GroupRole_Id)
                .Index(t => t.GroupMember_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GroupRoles", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.GroupRoleGroupMembers", "GroupMember_Id", "dbo.GroupMembers");
            DropForeignKey("dbo.GroupRoleGroupMembers", "GroupRole_Id", "dbo.GroupRoles");
            DropForeignKey("dbo.GroupMembers", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.GroupMembers", "CharacterId", "dbo.Characters");
            DropForeignKey("dbo.Sessions", "UserId", "dbo.Users");
            DropForeignKey("dbo.Characters", "UserId", "dbo.Users");
            DropForeignKey("dbo.Characters", "StyleId", "dbo.Styles");
            DropForeignKey("dbo.Skills", "CharacterId", "dbo.Characters");
            DropIndex("dbo.GroupRoleGroupMembers", new[] { "GroupMember_Id" });
            DropIndex("dbo.GroupRoleGroupMembers", new[] { "GroupRole_Id" });
            DropIndex("dbo.GroupRoles", new[] { "GroupId" });
            DropIndex("dbo.GroupMembers", new[] { "GroupId" });
            DropIndex("dbo.GroupMembers", new[] { "CharacterId" });
            DropIndex("dbo.Sessions", new[] { "UserId" });
            DropIndex("dbo.Skills", new[] { "CharacterId" });
            DropIndex("dbo.Characters", new[] { "StyleId" });
            DropIndex("dbo.Characters", new[] { "UserId" });
            DropTable("dbo.GroupRoleGroupMembers");
            DropTable("dbo.GroupRoles");
            DropTable("dbo.Groups");
            DropTable("dbo.GroupMembers");
            DropTable("dbo.Sessions");
            DropTable("dbo.Users");
            DropTable("dbo.Styles");
            DropTable("dbo.Skills");
            DropTable("dbo.Characters");
        }
    }
}
