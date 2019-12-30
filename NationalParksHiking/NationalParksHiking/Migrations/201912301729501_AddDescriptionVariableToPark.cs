namespace NationalParksHiking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDescriptionVariableToPark : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Parks", "ParkDescription", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Parks", "ParkDescription");
        }
    }
}
