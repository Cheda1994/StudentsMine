namespace StudentsMine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SomeChanges : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Conditions", "Until", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Conditions", "Until", c => c.DateTime(nullable: false));
        }
    }
}
