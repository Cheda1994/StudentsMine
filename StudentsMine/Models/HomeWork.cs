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
            this.Create = DateTime.Now;
            this.Course = cource;
            InitProjects(cource.Students);
        }
        public List<FileData> Attachments { get; set; }
        public virtual Course Course { get; set; }
        public virtual Condition Condition { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
        public DateTime? Create { get; set; }

        public void  InitProjects(Course course) 
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

        public void SendRemember()
        {
            string emails = "";
            foreach (var student in this.Course.Students)
            {
                emails += student.Email + " ";
            }
            string emailBody = "Hi , there is " + this.Course.Teacher.Name + " fom course "+ this.Course.Name+". <br /> I want to remember you about home work '"+ this.Title +"'";
            Mailer.Mailer mail = new Mailer.Mailer();
            mail.SetGeter(emails);
            mail.SetTitleAndBody("Home work remember", emailBody);
            mail.Send();
        }
    }


}