namespace com.BookSpider.DomainService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Key_Attribute_for_MenuItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MenuItemInfoes", "Context", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MenuItemInfoes", "Context");
        }
    }
}
