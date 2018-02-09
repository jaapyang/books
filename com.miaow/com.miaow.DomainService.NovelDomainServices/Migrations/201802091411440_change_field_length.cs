namespace com.miaow.DomainService.NovelDomainServices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class change_field_length : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ChapterModels", "Novel_Id", "dbo.NovelModels");
            DropIndex("dbo.ChapterModels", new[] { "Novel_Id" });
            RenameColumn(table: "dbo.ChapterModels", name: "Novel_Id", newName: "NovelId");
            AlterColumn("dbo.NovelModels", "NovelName", c => c.String(maxLength: 50));
            AlterColumn("dbo.NovelModels", "MenuUrl", c => c.String(maxLength: 200));
            AlterColumn("dbo.ChapterModels", "Title", c => c.String(maxLength: 50));
            AlterColumn("dbo.ChapterModels", "Url", c => c.String(maxLength: 200));
            AlterColumn("dbo.ChapterModels", "NovelId", c => c.Int(nullable: false));
            CreateIndex("dbo.ChapterModels", "NovelId");
            AddForeignKey("dbo.ChapterModels", "NovelId", "dbo.NovelModels", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChapterModels", "NovelId", "dbo.NovelModels");
            DropIndex("dbo.ChapterModels", new[] { "NovelId" });
            AlterColumn("dbo.ChapterModels", "NovelId", c => c.Int());
            AlterColumn("dbo.ChapterModels", "Url", c => c.String());
            AlterColumn("dbo.ChapterModels", "Title", c => c.String());
            AlterColumn("dbo.NovelModels", "MenuUrl", c => c.String());
            AlterColumn("dbo.NovelModels", "NovelName", c => c.String());
            RenameColumn(table: "dbo.ChapterModels", name: "NovelId", newName: "Novel_Id");
            CreateIndex("dbo.ChapterModels", "Novel_Id");
            AddForeignKey("dbo.ChapterModels", "Novel_Id", "dbo.NovelModels", "Id");
        }
    }
}
