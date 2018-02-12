namespace StudentsMine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGuidToFile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FileDatas", "Guid", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FileDatas", "Guid");
        }
    }
}
