namespace NationalParksHiking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddReviewAmountAndAverageToTrailsModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HikingTrails", "TotalUserReviews", c => c.Int(nullable: false));
            AddColumn("dbo.HikingTrails", "AverageUserRating", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.HikingTrails", "AverageUserRating");
            DropColumn("dbo.HikingTrails", "TotalUserReviews");
        }
    }
}
