namespace StudentsMine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Teacher_Id = c.Int(),
                        BaseAccessProps_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teachers", t => t.Teacher_Id)
                .ForeignKey("dbo.StudentAccessProps", t => t.BaseAccessProps_Id)
                .Index(t => t.Teacher_Id)
                .Index(t => t.BaseAccessProps_Id);
            
            CreateTable(
                "dbo.StudentAccessProps",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AccessToCourse = c.Boolean(nullable: false),
                        CanUploadFiles = c.Boolean(nullable: false),
                        CanDownloadFiles = c.Boolean(nullable: false),
                        AccessToHomeWork = c.Boolean(nullable: false),
                        Course_Id = c.Int(),
                        Student_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.Course_Id)
                .ForeignKey("dbo.Students", t => t.Student_Id)
                .Index(t => t.Course_Id)
                .Index(t => t.Student_Id);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsActive = c.Boolean(nullable: false),
                        Email = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserName = c.String(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        Role = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.LoginProvider, t.ProviderKey })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Mark = c.Int(nullable: false),
                        IsPublic = c.Boolean(nullable: false),
                        IsHomeWork = c.Boolean(nullable: false),
                        IsUploaded = c.Boolean(nullable: false),
                        HomeWork_Id = c.Int(),
                        Author_Id = c.Int(),
                        File_Id = c.Int(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        Course_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Students", t => t.Author_Id)
                .ForeignKey("dbo.FileDatas", t => t.File_Id)
                .ForeignKey("dbo.HomeWork", t => t.HomeWork_Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.Courses", t => t.Course_Id)
                .Index(t => t.HomeWork_Id)
                .Index(t => t.Author_Id)
                .Index(t => t.File_Id)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Course_Id);
            
            CreateTable(
                "dbo.FileDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Guid = c.Guid(nullable: false),
                        Name = c.String(),
                        Data = c.Binary(),
                        Format = c.String(),
                        HomeWork_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.HomeWork", t => t.HomeWork_Id)
                .Index(t => t.HomeWork_Id);
            
            CreateTable(
                "dbo.HomeWork",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Create = c.DateTime(),
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
                "dbo.Conditions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HasRequiredDate = c.Boolean(nullable: false),
                        Until = c.DateTime(),
                        IsBlocked = c.Boolean(nullable: false),
                        HasRequiredFormat = c.Boolean(nullable: false),
                        RequiredFormat = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Teachers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
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
            
            CreateTable(
                "dbo.StudentCourses",
                c => new
                    {
                        Student_Id = c.Int(nullable: false),
                        Course_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Student_Id, t.Course_Id })
                .ForeignKey("dbo.Students", t => t.Student_Id, cascadeDelete: true)
                .ForeignKey("dbo.Courses", t => t.Course_Id, cascadeDelete: true)
                .Index(t => t.Student_Id)
                .Index(t => t.Course_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderToCourses", "Student_Id", "dbo.Students");
            DropForeignKey("dbo.OrderToCourses", "Course_Id", "dbo.Courses");
            DropForeignKey("dbo.Projects", "Course_Id", "dbo.Courses");
            DropForeignKey("dbo.Courses", "BaseAccessProps_Id", "dbo.StudentAccessProps");
            DropForeignKey("dbo.StudentAccessProps", "Student_Id", "dbo.Students");
            DropForeignKey("dbo.StudentCourses", "Course_Id", "dbo.Courses");
            DropForeignKey("dbo.StudentCourses", "Student_Id", "dbo.Students");
            DropForeignKey("dbo.Students", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Courses", "Teacher_Id", "dbo.Teachers");
            DropForeignKey("dbo.Teachers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Projects", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Projects", "HomeWork_Id", "dbo.HomeWork");
            DropForeignKey("dbo.HomeWork", "Course_Id", "dbo.Courses");
            DropForeignKey("dbo.HomeWork", "Condition_Id", "dbo.Conditions");
            DropForeignKey("dbo.FileDatas", "HomeWork_Id", "dbo.HomeWork");
            DropForeignKey("dbo.Projects", "File_Id", "dbo.FileDatas");
            DropForeignKey("dbo.Projects", "Author_Id", "dbo.Students");
            DropForeignKey("dbo.AspNetUserClaims", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.StudentAccessProps", "Course_Id", "dbo.Courses");
            DropIndex("dbo.StudentCourses", new[] { "Course_Id" });
            DropIndex("dbo.StudentCourses", new[] { "Student_Id" });
            DropIndex("dbo.OrderToCourses", new[] { "Student_Id" });
            DropIndex("dbo.OrderToCourses", new[] { "Course_Id" });
            DropIndex("dbo.Teachers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.HomeWork", new[] { "Course_Id" });
            DropIndex("dbo.HomeWork", new[] { "Condition_Id" });
            DropIndex("dbo.FileDatas", new[] { "HomeWork_Id" });
            DropIndex("dbo.Projects", new[] { "Course_Id" });
            DropIndex("dbo.Projects", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Projects", new[] { "File_Id" });
            DropIndex("dbo.Projects", new[] { "Author_Id" });
            DropIndex("dbo.Projects", new[] { "HomeWork_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "User_Id" });
            DropIndex("dbo.Students", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.StudentAccessProps", new[] { "Student_Id" });
            DropIndex("dbo.StudentAccessProps", new[] { "Course_Id" });
            DropIndex("dbo.Courses", new[] { "BaseAccessProps_Id" });
            DropIndex("dbo.Courses", new[] { "Teacher_Id" });
            DropTable("dbo.StudentCourses");
            DropTable("dbo.OrderToCourses");
            DropTable("dbo.Teachers");
            DropTable("dbo.Conditions");
            DropTable("dbo.HomeWork");
            DropTable("dbo.FileDatas");
            DropTable("dbo.Projects");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Students");
            DropTable("dbo.StudentAccessProps");
            DropTable("dbo.Courses");
        }
    }
}
