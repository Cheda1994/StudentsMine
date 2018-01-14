using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentsMine.Models
{
    public class HomeWork
    {
        public HomeWork() { }
        public HomeWork(Course cource)
        {
            this.Course = cource;
            InitProjects(cource.Students);
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public virtual ICollection<FileData> Attachments { get; set; }
        public bool HasCondition { get; set; }
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

    public class HomeWorkListView
    {
        public HomeWorkListView(Project project)
        {
            this.Id = project.Id;
            this.Title = project.HomeWork.Title;
            this.Description = project.HomeWork.Description;
            this.HasCondition = project.HomeWork.HasCondition;
            if (project.HomeWork.HasCondition)
            {
                this.ConditionUntil = project.HomeWork.Condition.Until;
            }
            this.Mark = project.Mark;
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool HasCondition { get; set; }
        public DateTime? ConditionUntil { get; set; }
        public int Mark { get; set; }
    }

    public class HomeWorkPresentation{
        public HomeWorkPresentation(Project project)
        {
            this.Id = project.Id;
            this.Title = project.HomeWork.Title;
            this.Description = project.HomeWork.Description;
            this.Mark = project.Mark;
            this.HasCondition = project.HomeWork.HasCondition;
            if (project.HomeWork.HasCondition)
            {
                this.Condition = project.HomeWork.Condition;
            }
            this.Attachments = project.HomeWork.Attachments;
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Mark { get; set; }
        public bool IsBlocked { get; set; }
        public bool HasCondition { get; set; }
        public Condition Condition { get; set; }
        public ICollection<FileData> Attachments { get; set; }
    }
}