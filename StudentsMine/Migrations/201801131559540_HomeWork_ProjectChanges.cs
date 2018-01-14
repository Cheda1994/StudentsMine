namespace StudentsMine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HomeWork_ProjectChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "IsHomeWork", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projects", "IsHomeWork");
        }
    }
}
