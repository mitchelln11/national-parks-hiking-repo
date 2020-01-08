namespace NationalParksHiking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedVisitedBoolFromWishlist : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.HikerParkWishlists", "Visited");
        }
        
        public override void Down()
        {
            AddColumn("dbo.HikerParkWishlists", "Visited", c => c.Boolean(nullable: false));
        }
    }
}
