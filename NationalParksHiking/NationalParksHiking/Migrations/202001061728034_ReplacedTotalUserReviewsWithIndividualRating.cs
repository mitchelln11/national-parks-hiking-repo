namespace NationalParksHiking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReplacedTotalUserReviewsWithIndividualRating : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HikerTrailRatings", "IndividualRating", c => c.Int(nullable: false));
            DropColumn("dbo.HikerTrailRatings", "TotalUserReviews");
        }
        
        public override void Down()
        {
            AddColumn("dbo.HikerTrailRatings", "TotalUserReviews", c => c.Int(nullable: false));
            DropColumn("dbo.HikerTrailRatings", "IndividualRating");
        }
    }
}
