namespace NationalParksHiking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStateListToModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StatesLists",
                c => new
                    {
                        StateId = c.Int(nullable: false, identity: true),
                        StateName = c.String(),
                    })
                .PrimaryKey(t => t.StateId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.StatesLists");
        }
    }
}
