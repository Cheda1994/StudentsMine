namespace StudentsMine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CascadeMigrateonDelete : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FileDatas", "HomeWork_Id", "dbo.HomeWork");
            AddForeignKey("dbo.FileDatas", "HomeWork_Id", "dbo.HomeWork", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FileDatas", "HomeWork_Id", "dbo.HomeWork");
            AddForeignKey("dbo.FileDatas", "HomeWork_Id", "dbo.HomeWork", "Id");
        }
    }
}
