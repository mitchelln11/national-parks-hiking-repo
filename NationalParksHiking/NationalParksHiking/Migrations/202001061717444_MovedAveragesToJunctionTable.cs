namespace NationalParksHiking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MovedAveragesToJunctionTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HikerTrailRatings", "TotalUserReviews", c => c.Int(nullable: false));
            AddColumn("dbo.HikerTrailRatings", "AverageUserRating", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.HikerTrailRatings", "RatingAmt");
            DropColumn("dbo.HikingTrails", "TotalUserReviews");
            DropColumn("dbo.HikingTrails", "AverageUserRating");
        }
        
        public override void Down()
        {
            AddColumn("dbo.HikingTrails", "AverageUserRating", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.HikingTrails", "TotalUserReviews", c => c.Int(nullable: false));
            AddColumn("dbo.HikerTrailRatings", "RatingAmt", c => c.Int(nullable: false));
            DropColumn("dbo.HikerTrailRatings", "AverageUserRating");
            DropColumn("dbo.HikerTrailRatings", "TotalUserReviews");
        }
    }
}
