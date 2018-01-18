using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentsMine.Models
{

    public class HomeWork : HomeWorkTemplate
    {
        public HomeWork() { }
        public HomeWork(Course cource)
        {
            this.Course = cource;
            InitProjects(cource.Students);
        }
        public virtual ICollection<FileData> Attachments { get; set; }
        public Course Course { get; set; }
        public virtual Condition Condition { get; set; }
        public ICollection<Project> Projects { get; set; }

        public void InitProjects(Course course) 
        {
            this.Course = course;
            InitProjects(course.Students);
        }

        private void InitProjects(ICollection<Student> students)
        {
            this.Projects = new List<Project>(); 
            foreach (var student in students)
            {
                Project project = new Project();
                project.Author = student;
                project.IsHomeWork = true;
                this.Projects.Add(project);
            }
        }
    }


}