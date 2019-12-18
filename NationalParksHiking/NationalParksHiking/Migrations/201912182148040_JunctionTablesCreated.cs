namespace NationalParksHiking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JunctionTablesCreated : DbMigration
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
                .ForeignKey("dbo.Trails", t => t.TrailId, cascadeDelete: true)
                .Index(t => t.HikerId)
                .Index(t => t.TrailId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HikerTrailRatings", "TrailId", "dbo.Trails");
            DropForeignKey("dbo.HikerTrailRatings", "HikerId", "dbo.Hikers");
            DropForeignKey("dbo.HikerParkWishlists", "ParkId", "dbo.Parks");
            DropForeignKey("dbo.HikerParkWishlists", "HikerId", "dbo.Hikers");
            DropIndex("dbo.HikerTrailRatings", new[] { "TrailId" });
            DropIndex("dbo.HikerTrailRatings", new[] { "HikerId" });
            DropIndex("dbo.HikerParkWishlists", new[] { "ParkId" });
            DropIndex("dbo.HikerParkWishlists", new[] { "HikerId" });
            DropTable("dbo.HikerTrailRatings");
            DropTable("dbo.HikerParkWishlists");
        }
    }
}
