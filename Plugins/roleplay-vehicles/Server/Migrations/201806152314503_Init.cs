namespace Roleplay.Vehicles.Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Vehicles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        IsHoldable = c.Boolean(nullable: false, storeType: "bit"),
                        Position_X = c.Single(nullable: false),
                        Position_Y = c.Single(nullable: false),
                        Position_Z = c.Single(nullable: false),
                        Hash = c.Long(nullable: false),
                        Handle = c.Int(),
                        TrackingUserId = c.Guid(nullable: false),
                        NetId = c.Int(),
                        VIN = c.String(maxLength: 1000, unicode: false),
                        LicensePlate = c.String(maxLength: 1000, unicode: false),
                        BodyHealth = c.Single(nullable: false),
                        EngineHealth = c.Single(nullable: false),
                        DirtLevel = c.Single(nullable: false),
                        FuelLevel = c.Single(nullable: false),
                        OilLevel = c.Single(nullable: false),
                        PetrolTankHealth = c.Single(nullable: false),
                        TowingCraneRaisedAmount = c.Single(nullable: false),
                        PosX = c.Single(nullable: false),
                        PosY = c.Single(nullable: false),
                        PosZ = c.Single(nullable: false),
                        Heading = c.Single(nullable: false),
                        HasAlarm = c.Boolean(nullable: false, storeType: "bit"),
                        IsAlarmed = c.Boolean(nullable: false, storeType: "bit"),
                        IsAlarmSounding = c.Boolean(nullable: false, storeType: "bit"),
                        HasLock = c.Boolean(nullable: false, storeType: "bit"),
                        IsDriveable = c.Boolean(nullable: false, storeType: "bit"),
                        IsEngineRunning = c.Boolean(nullable: false, storeType: "bit"),
                        HasSeatbelts = c.Boolean(nullable: false, storeType: "bit"),
                        IsHighBeamsOn = c.Boolean(nullable: false, storeType: "bit"),
                        IsLightsOn = c.Boolean(nullable: false, storeType: "bit"),
                        IsInteriorLightOn = c.Boolean(nullable: false, storeType: "bit"),
                        IsSearchLightOn = c.Boolean(nullable: false, storeType: "bit"),
                        IsTaxiLightOn = c.Boolean(nullable: false, storeType: "bit"),
                        IsLeftIndicatorLightOn = c.Boolean(nullable: false, storeType: "bit"),
                        IsRightIndicatorLightOn = c.Boolean(nullable: false, storeType: "bit"),
                        IsFrontBumperBrokenOff = c.Boolean(nullable: false, storeType: "bit"),
                        IsRearBumperBrokenOff = c.Boolean(nullable: false, storeType: "bit"),
                        IsLeftHeadLightBroken = c.Boolean(nullable: false, storeType: "bit"),
                        IsRightHeadLightBroken = c.Boolean(nullable: false, storeType: "bit"),
                        IsRadioEnabled = c.Boolean(nullable: false, storeType: "bit"),
                        IsRoofOpen = c.Boolean(nullable: false, storeType: "bit"),
                        HasRoof = c.Boolean(nullable: false, storeType: "bit"),
                        IsVehicleConvertible = c.Boolean(nullable: false, storeType: "bit"),
                        NeedsToBeHotwired = c.Boolean(nullable: false, storeType: "bit"),
                        CanTiresBurst = c.Boolean(nullable: false, storeType: "bit"),
                        PrimaryColor_StockColor = c.Int(nullable: false),
                        PrimaryColor_CustomColor_R = c.Byte(),
                        PrimaryColor_CustomColor_G = c.Byte(),
                        PrimaryColor_CustomColor_B = c.Byte(),
                        PrimaryColor_CustomColor_A = c.Byte(),
                        PrimaryColor_IsCustom = c.Boolean(nullable: false, storeType: "bit"),
                        SecondaryColor_StockColor = c.Int(nullable: false),
                        SecondaryColor_CustomColor_R = c.Byte(),
                        SecondaryColor_CustomColor_G = c.Byte(),
                        SecondaryColor_CustomColor_B = c.Byte(),
                        SecondaryColor_CustomColor_A = c.Byte(),
                        SecondaryColor_IsCustom = c.Boolean(nullable: false, storeType: "bit"),
                        PearescentColor = c.Int(nullable: false),
                        DashboardColor = c.Int(nullable: false),
                        RimColor = c.Int(nullable: false),
                        NeonColor_R = c.Byte(),
                        NeonColor_G = c.Byte(),
                        NeonColor_B = c.Byte(),
                        NeonColor_A = c.Byte(),
                        NeonPositions = c.Int(nullable: false),
                        TireSmokeColor_R = c.Byte(),
                        TireSmokeColor_G = c.Byte(),
                        TireSmokeColor_B = c.Byte(),
                        TireSmokeColor_A = c.Byte(),
                        TrimColor = c.Int(nullable: false),
                        WindowTint = c.Int(nullable: false),
                        LockStatus = c.Int(nullable: false),
                        RadioStation = c.Int(nullable: false),
                        Class = c.Int(nullable: false),
                        Discriminator = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        TowedVehicle_Id = c.Guid(),
                        Trailer_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Vehicles", t => t.TowedVehicle_Id)
                .ForeignKey("dbo.Vehicles", t => t.Trailer_Id)
                .Index(t => t.TowedVehicle_Id)
                .Index(t => t.Trailer_Id);
            
            CreateTable(
                "dbo.VehicleDoors",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Index = c.Int(nullable: false),
                        IsOpen = c.Boolean(nullable: false, storeType: "bit"),
                        IsBroken = c.Boolean(nullable: false, storeType: "bit"),
                        Angle = c.Single(nullable: false),
                        Vehicle_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Vehicles", t => t.Vehicle_Id)
                .Index(t => t.Vehicle_Id);
            
            CreateTable(
                "dbo.VehicleExtras",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Index = c.Int(nullable: false),
                        IsOn = c.Boolean(nullable: false, storeType: "bit"),
                        VehicleId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Vehicles", t => t.VehicleId, cascadeDelete: true)
                .Index(t => t.VehicleId);
            
            CreateTable(
                "dbo.VehicleMods",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Index = c.Int(nullable: false),
                        Name = c.String(maxLength: 1000, unicode: false),
                        TypeName = c.String(maxLength: 1000, unicode: false),
                        Count = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        Vehicle_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Vehicles", t => t.Vehicle_Id)
                .Index(t => t.Vehicle_Id);
            
            CreateTable(
                "dbo.VehicleSeats",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Index = c.Int(nullable: false),
                        Character_Id = c.Guid(),
                        Vehicle_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Characters", t => t.Character_Id)
                .ForeignKey("dbo.Vehicles", t => t.Vehicle_Id)
                .Index(t => t.Character_Id)
                .Index(t => t.Vehicle_Id);
            
            CreateTable(
                "dbo.VehicleWheels",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Type = c.Int(nullable: false),
                        Position = c.Int(nullable: false),
                        Index = c.Int(nullable: false),
                        IsBurst = c.Boolean(nullable: false, storeType: "bit"),
                        Vehicle_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Vehicles", t => t.Vehicle_Id)
                .Index(t => t.Vehicle_Id);
            
            CreateTable(
                "dbo.VehicleWindows",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Index = c.Int(nullable: false),
                        IsIntact = c.Boolean(nullable: false, storeType: "bit"),
                        IsRolledDown = c.Boolean(nullable: false, storeType: "bit"),
                        Vehicle_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Vehicles", t => t.Vehicle_Id)
                .Index(t => t.Vehicle_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vehicles", "Trailer_Id", "dbo.Vehicles");
            DropForeignKey("dbo.Vehicles", "TowedVehicle_Id", "dbo.Vehicles");
            DropForeignKey("dbo.VehicleWindows", "Vehicle_Id", "dbo.Vehicles");
            DropForeignKey("dbo.VehicleWheels", "Vehicle_Id", "dbo.Vehicles");
            DropForeignKey("dbo.VehicleSeats", "Vehicle_Id", "dbo.Vehicles");
            DropForeignKey("dbo.VehicleSeats", "Character_Id", "dbo.Characters");
            DropForeignKey("dbo.VehicleMods", "Vehicle_Id", "dbo.Vehicles");
            DropForeignKey("dbo.VehicleExtras", "VehicleId", "dbo.Vehicles");
            DropForeignKey("dbo.VehicleDoors", "Vehicle_Id", "dbo.Vehicles");
            DropIndex("dbo.VehicleWindows", new[] { "Vehicle_Id" });
            DropIndex("dbo.VehicleWheels", new[] { "Vehicle_Id" });
            DropIndex("dbo.VehicleSeats", new[] { "Vehicle_Id" });
            DropIndex("dbo.VehicleSeats", new[] { "Character_Id" });
            DropIndex("dbo.VehicleMods", new[] { "Vehicle_Id" });
            DropIndex("dbo.VehicleExtras", new[] { "VehicleId" });
            DropIndex("dbo.VehicleDoors", new[] { "Vehicle_Id" });
            DropIndex("dbo.Vehicles", new[] { "Trailer_Id" });
            DropIndex("dbo.Vehicles", new[] { "TowedVehicle_Id" });
            DropTable("dbo.VehicleWindows");
            DropTable("dbo.VehicleWheels");
            DropTable("dbo.VehicleSeats");
            DropTable("dbo.VehicleMods");
            DropTable("dbo.VehicleExtras");
            DropTable("dbo.VehicleDoors");
            DropTable("dbo.Vehicles");
        }
    }
}
