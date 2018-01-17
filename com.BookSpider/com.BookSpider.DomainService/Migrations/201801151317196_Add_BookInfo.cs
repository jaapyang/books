namespace com.BookSpider.DomainService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_BookInfo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BookInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BookName = c.String(),
                        MenuUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MenuItemInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Url = c.String(),
                        Title = c.String(),
                        BookId = c.Int(nullable: false),
                        SortId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BookInfoes", t => t.BookId, cascadeDelete: true)
                .Index(t => t.BookId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MenuItemInfoes", "BookId", "dbo.BookInfoes");
            DropIndex("dbo.MenuItemInfoes", new[] { "BookId" });
            DropTable("dbo.MenuItemInfoes");
            DropTable("dbo.BookInfoes");
        }
    }
}
