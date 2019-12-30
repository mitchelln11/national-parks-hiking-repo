namespace NationalParksHiking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddParkForeignKeysToTrailsForLatLng : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HikingTrails", "ParkLat", c => c.String());
            AddColumn("dbo.HikingTrails", "ParkLng", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.HikingTrails", "ParkLng");
            DropColumn("dbo.HikingTrails", "ParkLat");
        }
    }
}
