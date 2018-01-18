namespace StudentsMine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldsToFileData : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FileDatas", "Name", c => c.String());
            AddColumn("dbo.FileDatas", "Data", c => c.Binary());
            AddColumn("dbo.FileDatas", "Resolution", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FileDatas", "Resolution");
            DropColumn("dbo.FileDatas", "Data");
            DropColumn("dbo.FileDatas", "Name");
        }
    }
}
