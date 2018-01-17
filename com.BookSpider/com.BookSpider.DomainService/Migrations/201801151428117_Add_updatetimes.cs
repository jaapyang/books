namespace com.BookSpider.DomainService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_updatetimes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MenuItemInfoes", "CreatedDateTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.MenuItemInfoes", "LastUpdateTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MenuItemInfoes", "LastUpdateTime");
            DropColumn("dbo.MenuItemInfoes", "CreatedDateTime");
        }
    }
}
