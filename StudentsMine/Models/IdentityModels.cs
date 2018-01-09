﻿using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Data.Entity;

namespace StudentsMine.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() {
            Projects = new List<Project>();
        }
        public virtual Student Student { get; set; }
        public virtual Teacher Teacher { get; set; }
        public string Role { get; set; }
        public ICollection<Project> Projects { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<OrderToCourse> OrdersToCourse { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<HomeWork> Homeworks { get; set; }
        public DbSet<Project> Projects { get; set; }
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}