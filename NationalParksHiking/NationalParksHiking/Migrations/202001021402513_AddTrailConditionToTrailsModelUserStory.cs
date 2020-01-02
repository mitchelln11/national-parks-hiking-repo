namespace NationalParksHiking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTrailConditionToTrailsModelUserStory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HikingTrails", "TrailCondition", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.HikingTrails", "TrailCondition");
        }
    }
}
