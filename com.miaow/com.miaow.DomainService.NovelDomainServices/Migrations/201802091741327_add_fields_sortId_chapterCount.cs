namespace com.miaow.DomainService.NovelDomainServices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_fields_sortId_chapterCount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NovelModels", "MaxChapterIndex", c => c.Int(nullable: false));
            AddColumn("dbo.ChapterModels", "SortId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ChapterModels", "SortId");
            DropColumn("dbo.NovelModels", "MaxChapterIndex");
        }
    }
}
