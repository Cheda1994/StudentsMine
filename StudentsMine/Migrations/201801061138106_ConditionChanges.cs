namespace StudentsMine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConditionChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Conditions", "Until", c => c.DateTime(nullable: false));
            AddColumn("dbo.Conditions", "IsBlocked", c => c.Boolean(nullable: false));
            AddColumn("dbo.Conditions", "HasRequiredFormat", c => c.Boolean(nullable: false));
            AddColumn("dbo.Conditions", "RequiredFormat", c => c.String());
            AddColumn("dbo.Projects", "Mark", c => c.Int(nullable: false));
            AddColumn("dbo.Projects", "IsPublic", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projects", "IsPublic");
            DropColumn("dbo.Projects", "Mark");
            DropColumn("dbo.Conditions", "RequiredFormat");
            DropColumn("dbo.Conditions", "HasRequiredFormat");
            DropColumn("dbo.Conditions", "IsBlocked");
            DropColumn("dbo.Conditions", "Until");
        }
    }
}
