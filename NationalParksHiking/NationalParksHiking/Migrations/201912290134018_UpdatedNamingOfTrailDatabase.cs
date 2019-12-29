namespace NationalParksHiking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedNamingOfTrailDatabase : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.HikingTrails", "TrailLength", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.HikingTrails", "TrailLength", c => c.String());
        }
    }
}
