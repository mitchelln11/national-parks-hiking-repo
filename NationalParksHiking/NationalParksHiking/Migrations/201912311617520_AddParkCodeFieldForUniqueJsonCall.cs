namespace NationalParksHiking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddParkCodeFieldForUniqueJsonCall : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Parks", "ParkCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Parks", "ParkCode");
        }
    }
}
