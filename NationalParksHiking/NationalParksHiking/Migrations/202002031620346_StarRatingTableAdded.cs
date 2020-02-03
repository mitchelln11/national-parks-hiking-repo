namespace NationalParksHiking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StarRatingTableAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StarRatings",
                c => new
                    {
                        StarRatingId = c.Int(nullable: false, identity: true),
                        IndividualStarRating = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StarRatingId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.StarRatings");
        }
    }
}
