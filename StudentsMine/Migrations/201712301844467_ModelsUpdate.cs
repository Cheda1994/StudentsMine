namespace StudentsMine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelsUpdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Projects", "Teacher_Id", "dbo.Teachers");
            DropIndex("dbo.Projects", new[] { "Teacher_Id" });
            AddColumn("dbo.Projects", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Projects", "ApplicationUser_Id");
            AddForeignKey("dbo.Projects", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.Projects", "Teacher_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Projects", "Teacher_Id", c => c.Int());
            DropForeignKey("dbo.Projects", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Projects", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.Projects", "ApplicationUser_Id");
            CreateIndex("dbo.Projects", "Teacher_Id");
            AddForeignKey("dbo.Projects", "Teacher_Id", "dbo.Teachers", "Id");
        }
    }
}
