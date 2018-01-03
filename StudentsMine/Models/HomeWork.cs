using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentsMine.Models
{
    public class HomeWork
    {
        public HomeWork(Course cource)
        {
            this.Course = cource;
            InitProjects(cource.Students);
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ICollection<FileData> Attachments { get; set; }
        public bool HasCondition { get; set; }
        public Course Course { get; set; }
        public Condition Condition { get; set; }
        public ICollection<Project> Projects { get; set; }

        private void InitProjects(ICollection<Student> students)
        {
            this.Projects = new List<Project>(); 
            foreach (var student in students)
            {
                Project project = new Project();
                project.Author = student;
                this.Projects.Add(project);
            }
        }
    }


}