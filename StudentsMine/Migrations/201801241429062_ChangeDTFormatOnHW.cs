namespace StudentsMine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeDTFormatOnHW : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.HomeWork", "Create", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.HomeWork", "Create", c => c.DateTime(nullable: false));
        }
    }
}
