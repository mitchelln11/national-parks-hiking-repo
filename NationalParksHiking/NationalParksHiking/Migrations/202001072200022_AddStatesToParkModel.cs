namespace NationalParksHiking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStatesToParkModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HikerParkWishlists", "Visited", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.HikerParkWishlists", "Visited");
        }
    }
}
