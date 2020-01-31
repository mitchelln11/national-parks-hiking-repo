namespace NationalParksHiking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedStateListDatabase : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.StatesLists");
        }
        
        public override void Down()
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
    }
}
