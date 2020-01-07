namespace NationalParksHiking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddParkNameToWishlistForVisualParksAddingToWishlist : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HikerParkWishlists", "ParkName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.HikerParkWishlists", "ParkName");
        }
    }
}
