namespace StudentsMine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewModels_Cource_Homework1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Courses", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Students", "Course_Id", "dbo.Courses");
            DropForeignKey("dbo.Courses", "Teacher_Id", "dbo.Teachers");
            DropIndex("dbo.Courses", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Courses", new[] { "Teacher_Id" });
            DropIndex("dbo.Students", new[] { "Course_Id" });
            CreateTable(
                "dbo.CourseApplicationUsers",
                c => new
                    {
                        Course_Id = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Course_Id, t.ApplicationUser_Id })
                .ForeignKey("dbo.Courses", t => t.Course_Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .Index(t => t.Course_Id)
                .Index(t => t.ApplicationUser_Id);
            
            AlterColumn("dbo.Courses", "Teacher_Id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Courses", "Teacher_Id");
            AddForeignKey("dbo.Courses", "Teacher_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            DropColumn("dbo.Courses", "ApplicationUser_Id");
            DropColumn("dbo.Students", "Course_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Students", "Course_Id", c => c.Int());
            AddColumn("dbo.Courses", "ApplicationUser_Id", c => c.String(maxLength: 128));
            DropForeignKey("dbo.Courses", "Teacher_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.CourseApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.CourseApplicationUsers", "Course_Id", "dbo.Courses");
            DropIndex("dbo.CourseApplicationUsers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.CourseApplicationUsers", new[] { "Course_Id" });
            DropIndex("dbo.Courses", new[] { "Teacher_Id" });
            AlterColumn("dbo.Courses", "Teacher_Id", c => c.Int());
            DropTable("dbo.CourseApplicationUsers");
            CreateIndex("dbo.Students", "Course_Id");
            CreateIndex("dbo.Courses", "Teacher_Id");
            CreateIndex("dbo.Courses", "ApplicationUser_Id");
            AddForeignKey("dbo.Courses", "Teacher_Id", "dbo.Teachers", "Id");
            AddForeignKey("dbo.Students", "Course_Id", "dbo.Courses", "Id");
            AddForeignKey("dbo.Courses", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
