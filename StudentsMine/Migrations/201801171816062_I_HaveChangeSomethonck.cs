namespace StudentsMine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class I_HaveChangeSomethonck : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FileDatas", "Format", c => c.String());
            AddColumn("dbo.Conditions", "HasRequiredDate", c => c.Boolean(nullable: false));
            DropColumn("dbo.FileDatas", "Resolution");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FileDatas", "Resolution", c => c.String());
            DropColumn("dbo.Conditions", "HasRequiredDate");
            DropColumn("dbo.FileDatas", "Format");
        }
    }
}
