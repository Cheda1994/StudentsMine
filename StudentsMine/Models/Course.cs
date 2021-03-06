﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace StudentsMine.Models
{
    public class Course
    {
        public Course()
        {
            this.Students = new List<Student>();
            this.HomeWorks = new List<HomeWork>();
            this.ProjectsHub = new List<Project>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual Teacher Teacher { get; set; }
        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<StudentAccessProp> StudentAccessProps { get; set; }
        public ICollection<HomeWork> HomeWorks { get; set; }
        public ICollection<Project> ProjectsHub { get; set; }
        public virtual StudentAccessProp BaseAccessProps { get; set; }
    }

    public class AddCourseView
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
