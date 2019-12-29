namespace NationalParksHiking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FieldsAddedToTrailModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HikingTrails", "TrailSummary", c => c.String());
            AddColumn("dbo.HikingTrails", "TrailLength", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.HikingTrails", "TrailLength");
            DropColumn("dbo.HikingTrails", "TrailSummary");
        }
    }
}
