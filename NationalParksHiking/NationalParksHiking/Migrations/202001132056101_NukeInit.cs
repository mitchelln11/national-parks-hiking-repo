namespace NationalParksHiking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NukeInit : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HikerParkWishlists",
                c => new
                    {
                        HikerParkWishlistId = c.Int(nullable: false, identity: true),
                        HikerId = c.Int(nullable: false),
                        ParkId = c.Int(nullable: false),
                        ParkName = c.String(),
                    })
                .PrimaryKey(t => t.HikerParkWishlistId)
                .ForeignKey("dbo.Hikers", t => t.HikerId, cascadeDelete: true)
                .ForeignKey("dbo.Parks", t => t.ParkId, cascadeDelete: true)
                .Index(t => t.HikerId)
                .Index(t => t.ParkId);
            
            CreateTable(
                "dbo.Hikers",
                c => new
                    {
                        HikerId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        StreetAddress = c.String(),
                        City = c.String(),
                        State = c.String(),
                        Latitude = c.String(),
                        Longitude = c.String(),
                        ApplicationId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.HikerId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationId)
                .Index(t => t.ApplicationId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Parks",
                c => new
                    {
                        ParkId = c.Int(nullable: false, identity: true),
                        ParkName = c.String(),
                        ParkState = c.String(),
                        ParkLat = c.String(),
                        ParkLng = c.String(),
                        ParkDescription = c.String(),
                        ParkCode = c.String(),
                    })
                .PrimaryKey(t => t.ParkId);
            
            CreateTable(
                "dbo.HikerTrailRatings",
                c => new
                    {
                        HikerTrailRatingId = c.Int(nullable: false, identity: true),
                        HikerId = c.Int(nullable: false),
                        TrailId = c.Int(nullable: false),
                        IndividualRating = c.Int(nullable: false),
                        AverageUserRating = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.HikerTrailRatingId)
                .ForeignKey("dbo.Hikers", t => t.HikerId, cascadeDelete: true)
                .ForeignKey("dbo.HikingTrails", t => t.TrailId, cascadeDelete: true)
                .Index(t => t.HikerId)
                .Index(t => t.TrailId);
            
            CreateTable(
                "dbo.HikingTrails",
                c => new
                    {
                        TrailId = c.Int(nullable: false, identity: true),
                        TrailName = c.String(),
                        TrailDifficulty = c.String(),
                        TrailSummary = c.String(),
                        TrailLength = c.Double(nullable: false),
                        TrailCondition = c.String(),
                        HikingApiCode = c.String(),
                        ParkId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TrailId)
                .ForeignKey("dbo.Parks", t => t.ParkId, cascadeDelete: true)
                .Index(t => t.ParkId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.HikerTrailRatings", "TrailId", "dbo.HikingTrails");
            DropForeignKey("dbo.HikingTrails", "ParkId", "dbo.Parks");
            DropForeignKey("dbo.HikerTrailRatings", "HikerId", "dbo.Hikers");
            DropForeignKey("dbo.HikerParkWishlists", "ParkId", "dbo.Parks");
            DropForeignKey("dbo.HikerParkWishlists", "HikerId", "dbo.Hikers");
            DropForeignKey("dbo.Hikers", "ApplicationId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.HikingTrails", new[] { "ParkId" });
            DropIndex("dbo.HikerTrailRatings", new[] { "TrailId" });
            DropIndex("dbo.HikerTrailRatings", new[] { "HikerId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Hikers", new[] { "ApplicationId" });
            DropIndex("dbo.HikerParkWishlists", new[] { "ParkId" });
            DropIndex("dbo.HikerParkWishlists", new[] { "HikerId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.HikingTrails");
            DropTable("dbo.HikerTrailRatings");
            DropTable("dbo.Parks");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Hikers");
            DropTable("dbo.HikerParkWishlists");
        }
    }
}
