namespace NationalParksHiking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MovingClassesAroundToWorkOnDropDown : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Parks", "State");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Parks", "State", c => c.String());
        }
    }
}
