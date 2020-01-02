namespace NationalParksHiking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTrailConditionToString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.HikingTrails", "TrailCondition", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.HikingTrails", "TrailCondition", c => c.Double(nullable: false));
        }
    }
}
