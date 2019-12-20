namespace NationalParksHiking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HidingAddressNotNeeded : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Parks", "StreetAddress");
            DropColumn("dbo.Parks", "ParkCity");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Parks", "ParkCity", c => c.String());
            AddColumn("dbo.Parks", "StreetAddress", c => c.String());
        }
    }
}
