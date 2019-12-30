namespace NationalParksHiking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class thing : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.HikingTrails", "ParkLat");
            DropColumn("dbo.HikingTrails", "ParkLng");
        }
        
        public override void Down()
        {
            AddColumn("dbo.HikingTrails", "ParkLng", c => c.String());
            AddColumn("dbo.HikingTrails", "ParkLat", c => c.String());
        }
    }
}
