namespace StudentsMine.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using StudentsMine.App_Start;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<StudentsMine.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(StudentsMine.Models.ApplicationDbContext context)
        {
            InitRoles(context);
        }

        private void InitRoles(DbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>((StudentsMine.Models.ApplicationDbContext)context));
            if (!roleManager.RoleExists(ApplicationConstants.TEACHER))
            {
                var role = new IdentityRole();
                role.Name = ApplicationConstants.TEACHER;
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists(ApplicationConstants.STUDENT))
            {
                var role = new IdentityRole();
                role.Name = ApplicationConstants.STUDENT;
                roleManager.Create(role);
            }
        }
    }
}
