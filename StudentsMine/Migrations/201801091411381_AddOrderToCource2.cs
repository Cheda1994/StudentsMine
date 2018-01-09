namespace StudentsMine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrderToCource2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderToCourses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.Boolean(nullable: false),
                        Course_Id = c.Int(),
                        Student_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.Course_Id)
                .ForeignKey("dbo.Students", t => t.Student_Id)
                .Index(t => t.Course_Id)
                .Index(t => t.Student_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderToCourses", "Student_Id", "dbo.Students");
            DropForeignKey("dbo.OrderToCourses", "Course_Id", "dbo.Courses");
            DropIndex("dbo.OrderToCourses", new[] { "Student_Id" });
            DropIndex("dbo.OrderToCourses", new[] { "Course_Id" });
            DropTable("dbo.OrderToCourses");
        }
    }
}
