namespace NationalParksHiking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatesToDatabaseApparently : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.HikerTrailRatings", "AverageUserRating");
        }
        
        public override void Down()
        {
            AddColumn("dbo.HikerTrailRatings", "AverageUserRating", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
