using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentsMine.Models
{
    public class Student : UserTemplate
    {
        public Student() : base() 
        {
            IsActive = true;
        }

        public Student(CreateStudentView studentView)
            : this()
        {
            this.Email = studentView.Email;
            this.Name = studentView.Name;
        }

        public Student(string email , ApplicationUser user , Course cource) : this()
        {
            this.ApplicationUser = user;
            this.Courses.Add(cource);
        }

        public bool IsActive { get; set; }
    }

    public class CreateStudentView : RegisterViewModel
    {
        public string Name { get; set; }
    }

    public class OrderStudentToCourceView
    {
        public string Email {get;set;}
    }
    public class SudentRegistrationStatus
    {
        public SudentRegistrationStatus(CreateStudentView Student, bool Result, StudentRegResults status,  IEnumerable<string> ErrorMessage)
        {
            this.Status = status;
            this.Email = Student.Email;
            this.Name = Student.Name;
            this.Result = Result;
            if (Result)
            {
                this.ErrorMessage = "No errors";
            }
            else
            {
                foreach (var message in ErrorMessage)
                {
                    this.ErrorMessage += string.Format("Error: {0} ", message);
                }
            }
        }
        public string Email { get; set; }
        public string Name { get; set; }
        public bool Result { get; set; }
        public StudentRegResults Status { get; set; }
        public string ErrorMessage { get; set; }
        public List<OrderToCourse> OrdersToCourse { get; set; }
    }

    public class SudentAddToCourseStatus {
        public SudentAddToCourseStatus(string email , bool result , StudentRegResults status, IEnumerable<string> ErrorMessage)
        {
            this.Email = email;
            this.Result = result;
            this.Status = status;
            if (Result)
            {
                this.ErrorMessage = "No errors";
            }
            else
            {
                foreach (var message in ErrorMessage)
                {
                    this.ErrorMessage += string.Format("Error: {0} ", message);
                }
            }
        }
        public string Email { get; set; }
        public bool Result { get; set; }
        public StudentRegResults Status { get; set; }
        public string ErrorMessage { get; set; }
    }
    public enum StudentRegResults { OK, EmailExists, NoValideModel, Exception, AlreadyOrdered, AlreadyParticipate }
}