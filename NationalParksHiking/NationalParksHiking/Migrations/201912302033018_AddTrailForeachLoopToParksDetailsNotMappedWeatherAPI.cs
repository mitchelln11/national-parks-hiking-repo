namespace NationalParksHiking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTrailForeachLoopToParksDetailsNotMappedWeatherAPI : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Parks", "CurrentWeatherInfo_temperature");
            DropColumn("dbo.Parks", "CurrentWeatherInfo_wind");
            DropColumn("dbo.Parks", "CurrentWeatherInfo_condition");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Parks", "CurrentWeatherInfo_condition", c => c.String());
            AddColumn("dbo.Parks", "CurrentWeatherInfo_wind", c => c.Double(nullable: false));
            AddColumn("dbo.Parks", "CurrentWeatherInfo_temperature", c => c.Double(nullable: false));
        }
    }
}
