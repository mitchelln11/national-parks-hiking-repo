namespace NationalParksHiking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddWeatherValuesToParkModelNotSeeded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Parks", "CurrentWeatherInfo_temperature", c => c.Double(nullable: false));
            AddColumn("dbo.Parks", "CurrentWeatherInfo_wind", c => c.Double(nullable: false));
            AddColumn("dbo.Parks", "CurrentWeatherInfo_condition", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Parks", "CurrentWeatherInfo_condition");
            DropColumn("dbo.Parks", "CurrentWeatherInfo_wind");
            DropColumn("dbo.Parks", "CurrentWeatherInfo_temperature");
        }
    }
}
