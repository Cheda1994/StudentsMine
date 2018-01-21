namespace StudentsMine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUploadStatusToHomeWorkProject : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "IsUploaded", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projects", "IsUploaded");
        }
    }
}
