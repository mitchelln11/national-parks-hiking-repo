namespace NationalParksHiking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedIntToDecimalAverageRating : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.HikingTrails", "AverageUserRating", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.HikingTrails", "AverageUserRating", c => c.Int(nullable: false));
        }
    }
}
