namespace StudentsMine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCreateDateToHW : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HomeWork", "Create", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.HomeWork", "Create");
        }
    }
}
