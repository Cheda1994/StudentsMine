namespace StudentsMine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewModels_Cource_Homework : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.CourseStudents", newName: "StudentCourses");
            DropPrimaryKey("dbo.StudentCourses");
            AddPrimaryKey("dbo.StudentCourses", new[] { "Student_Id", "Course_Id" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.StudentCourses");
            AddPrimaryKey("dbo.StudentCourses", new[] { "Course_Id", "Student_Id" });
            RenameTable(name: "dbo.StudentCourses", newName: "CourseStudents");
        }
    }
}
