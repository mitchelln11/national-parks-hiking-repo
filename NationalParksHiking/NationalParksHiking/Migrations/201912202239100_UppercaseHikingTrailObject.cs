namespace NationalParksHiking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UppercaseHikingTrailObject : DbMigration
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
                    })
                .PrimaryKey(t => t.HikerId);
            
            CreateTable(
                "dbo.Parks",
                c => new
                    {
                        ParkId = c.Int(nullable: false, identity: true),
                        ParkName = c.String(),
                        ParkState = c.String(),
                        ParkLat = c.String(),
                        ParkLng = c.String(),
                    })
                .PrimaryKey(t => t.ParkId);
            
            CreateTable(
                "dbo.HikerTrailRatings",
                c => new
                    {
                        HikerTrailRatingId = c.Int(nullable: false, identity: true),
                        HikerId = c.Int(nullable: false),
                        TrailId = c.Int(nullable: false),
                        RatingAmt = c.Int(nullable: false),
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
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.HikerTrailRatings", "TrailId", "dbo.HikingTrails");
            DropForeignKey("dbo.HikingTrails", "ParkId", "dbo.Parks");
            DropForeignKey("dbo.HikerTrailRatings", "HikerId", "dbo.Hikers");
            DropForeignKey("dbo.HikerParkWishlists", "ParkId", "dbo.Parks");
            DropForeignKey("dbo.HikerParkWishlists", "HikerId", "dbo.Hikers");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.HikingTrails", new[] { "ParkId" });
            DropIndex("dbo.HikerTrailRatings", new[] { "TrailId" });
            DropIndex("dbo.HikerTrailRatings", new[] { "HikerId" });
            DropIndex("dbo.HikerParkWishlists", new[] { "ParkId" });
            DropIndex("dbo.HikerParkWishlists", new[] { "HikerId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.HikingTrails");
            DropTable("dbo.HikerTrailRatings");
            DropTable("dbo.Parks");
            DropTable("dbo.Hikers");
            DropTable("dbo.HikerParkWishlists");
        }
    }
}
