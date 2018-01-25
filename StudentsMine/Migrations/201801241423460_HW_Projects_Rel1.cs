namespace StudentsMine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HW_Projects_Rel1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Projects", "HomeWork_Id", "dbo.HomeWork");
            AddForeignKey("dbo.Projects", "HomeWork_Id", "dbo.HomeWork", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Projects", "HomeWork_Id", "dbo.HomeWork");
            AddForeignKey("dbo.Projects", "HomeWork_Id", "dbo.HomeWork", "Id", cascadeDelete: true);
        }
    }
}
