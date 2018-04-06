namespace StudentsMine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LengthTitle : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.HomeWork", "Title", c => c.String(maxLength: 13));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.HomeWork", "Title", c => c.String());
        }
    }
}
