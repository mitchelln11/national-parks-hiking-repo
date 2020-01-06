namespace NationalParksHiking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedHikingIdToMapped : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HikingTrails", "HikingApiCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.HikingTrails", "HikingApiCode");
        }
    }
}
