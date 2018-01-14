namespace StudentsMine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HomeWork_ProjectChanges2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "File_Id", c => c.Int());
            CreateIndex("dbo.Projects", "File_Id");
            AddForeignKey("dbo.Projects", "File_Id", "dbo.FileDatas", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Projects", "File_Id", "dbo.FileDatas");
            DropIndex("dbo.Projects", new[] { "File_Id" });
            DropColumn("dbo.Projects", "File_Id");
        }
    }
}
