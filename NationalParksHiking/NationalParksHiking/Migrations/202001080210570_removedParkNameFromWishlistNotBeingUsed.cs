namespace NationalParksHiking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedParkNameFromWishlistNotBeingUsed : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.HikerParkWishlists", "ParkName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.HikerParkWishlists", "ParkName", c => c.String());
        }
    }
}
