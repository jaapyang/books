namespace com.BookSpider.DomainService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class set_column_length : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BookInfoes", "BookName", c => c.String(maxLength: 200));
            AlterColumn("dbo.BookInfoes", "MenuUrl", c => c.String(maxLength: 200));
            AlterColumn("dbo.MenuItemInfoes", "Url", c => c.String(maxLength: 200));
            AlterColumn("dbo.MenuItemInfoes", "Title", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MenuItemInfoes", "Title", c => c.String());
            AlterColumn("dbo.MenuItemInfoes", "Url", c => c.String());
            AlterColumn("dbo.BookInfoes", "MenuUrl", c => c.String());
            AlterColumn("dbo.BookInfoes", "BookName", c => c.String());
        }
    }
}
