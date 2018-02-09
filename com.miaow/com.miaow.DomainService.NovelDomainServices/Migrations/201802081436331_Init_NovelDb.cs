namespace com.miaow.DomainService.NovelDomainServices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init_NovelDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NovelModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NovelName = c.String(),
                        MenuUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ChapterModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Url = c.String(),
                        Content = c.String(),
                        LastUpdatedTime = c.DateTime(nullable: false),
                        Novel_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NovelModels", t => t.Novel_Id)
                .Index(t => t.Novel_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChapterModels", "Novel_Id", "dbo.NovelModels");
            DropIndex("dbo.ChapterModels", new[] { "Novel_Id" });
            DropTable("dbo.ChapterModels");
            DropTable("dbo.NovelModels");
        }
    }
}
