namespace StudentsMine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitModels : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Projects", name: "Student_Id", newName: "Author_Id");
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Teacher_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teachers", t => t.Teacher_Id)
                .Index(t => t.Teacher_Id);
            
            CreateTable(
                "dbo.HomeWork",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        HasCondition = c.Boolean(nullable: false),
                        Condition_Id = c.Int(),
                        Course_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Conditions", t => t.Condition_Id)
                .ForeignKey("dbo.Courses", t => t.Course_Id)
                .Index(t => t.Condition_Id)
                .Index(t => t.Course_Id);
            
            CreateTable(
                "dbo.FileDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HomeWork_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.HomeWork", t => t.HomeWork_Id)
                .Index(t => t.HomeWork_Id);
            
            CreateTable(
                "dbo.Conditions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CourseStudents",
                c => new
                    {
                        Course_Id = c.Int(nullable: false),
                        Student_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Course_Id, t.Student_Id })
                .ForeignKey("dbo.Courses", t => t.Course_Id, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.Student_Id, cascadeDelete: true)
                .Index(t => t.Course_Id)
                .Index(t => t.Student_Id);
            
            AddColumn("dbo.Projects", "HomeWork_Id", c => c.Int());
            AddColumn("dbo.Projects", "Course_Id", c => c.Int());
            AddColumn("dbo.Projects", "Teacher_Id", c => c.Int());
            AlterColumn("dbo.Students", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Teachers", "Name", c => c.String(nullable: false));
            CreateIndex("dbo.Projects", "HomeWork_Id");
            CreateIndex("dbo.Projects", "Course_Id");
            CreateIndex("dbo.Projects", "Teacher_Id");
            AddForeignKey("dbo.Projects", "HomeWork_Id", "dbo.HomeWork", "Id");
            AddForeignKey("dbo.Projects", "Course_Id", "dbo.Courses", "Id");
            AddForeignKey("dbo.Projects", "Teacher_Id", "dbo.Teachers", "Id");
            DropColumn("dbo.AspNetUsers", "Email");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Email", c => c.String());
            DropForeignKey("dbo.Projects", "Teacher_Id", "dbo.Teachers");
            DropForeignKey("dbo.Courses", "Teacher_Id", "dbo.Teachers");
            DropForeignKey("dbo.CourseStudents", "Student_Id", "dbo.Students");
            DropForeignKey("dbo.CourseStudents", "Course_Id", "dbo.Courses");
            DropForeignKey("dbo.Projects", "Course_Id", "dbo.Courses");
            DropForeignKey("dbo.Projects", "HomeWork_Id", "dbo.HomeWork");
            DropForeignKey("dbo.HomeWork", "Course_Id", "dbo.Courses");
            DropForeignKey("dbo.HomeWork", "Condition_Id", "dbo.Conditions");
            DropForeignKey("dbo.FileDatas", "HomeWork_Id", "dbo.HomeWork");
            DropIndex("dbo.Projects", new[] { "Teacher_Id" });
            DropIndex("dbo.Courses", new[] { "Teacher_Id" });
            DropIndex("dbo.CourseStudents", new[] { "Student_Id" });
            DropIndex("dbo.CourseStudents", new[] { "Course_Id" });
            DropIndex("dbo.Projects", new[] { "Course_Id" });
            DropIndex("dbo.Projects", new[] { "HomeWork_Id" });
            DropIndex("dbo.HomeWork", new[] { "Course_Id" });
            DropIndex("dbo.HomeWork", new[] { "Condition_Id" });
            DropIndex("dbo.FileDatas", new[] { "HomeWork_Id" });
            AlterColumn("dbo.Teachers", "Name", c => c.String());
            AlterColumn("dbo.Students", "Name", c => c.String());
            DropColumn("dbo.Projects", "Teacher_Id");
            DropColumn("dbo.Projects", "Course_Id");
            DropColumn("dbo.Projects", "HomeWork_Id");
            DropTable("dbo.CourseStudents");
            DropTable("dbo.Conditions");
            DropTable("dbo.FileDatas");
            DropTable("dbo.HomeWork");
            DropTable("dbo.Courses");
            RenameColumn(table: "dbo.Projects", name: "Author_Id", newName: "Student_Id");
        }
    }
}
