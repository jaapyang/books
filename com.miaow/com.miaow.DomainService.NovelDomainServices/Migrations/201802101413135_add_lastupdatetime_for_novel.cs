namespace com.miaow.DomainService.NovelDomainServices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_lastupdatetime_for_novel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NovelModels", "LastUpdateTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.NovelModels", "LastUpdateTime");
        }
    }
}
