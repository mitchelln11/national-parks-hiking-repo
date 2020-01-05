namespace NationalParksHiking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddApplicationIdAsForeignIdToHikersModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Hikers", "ApplicationId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Hikers", "ApplicationId");
            AddForeignKey("dbo.Hikers", "ApplicationId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Hikers", "ApplicationId", "dbo.AspNetUsers");
            DropIndex("dbo.Hikers", new[] { "ApplicationId" });
            DropColumn("dbo.Hikers", "ApplicationId");
        }
    }
}
