using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
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
        public int UIId { get; set; }
        public string Name { get; set; }

        public override RegistrationStatus Verify()
        {
            var status = base.Verify();
            if (this.Name.Length < 8 || this.Name.Length > 20)
            {
                status.Result = false;
                status.ErrorMessage += "The Name size is incorrect.";
            }
            return status;
        }
    }

    public class OrderStudentToCourceView
    {
        public int UIId { get; set; }
        public string Email {get;set;}
    }
    public class RegistrationStatus
    {
        public RegistrationStatus(RegisterViewModel model, bool Result, StudentRegResults status,  IEnumerable<string> ErrorMessage)
        {
            this.Status = status;
            this.Email = model.Email;
            this.Result = Result;
            if (!Result)
            {
                foreach (var message in ErrorMessage)
                {
                    this.ErrorMessage += string.Format("Error: {0} ", message);
                }
            }
        }
        public int UIId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public bool Result { get; set; }
        public StudentRegResults Status { get; set; }
        public string ErrorMessage { get; set; }

    }

    public class SudentAddToCourseStatus {
        public SudentAddToCourseStatus(string email , bool result , StudentRegResults status, IEnumerable<string> ErrorMessage , int UIId) :this( email ,  result ,  status,  ErrorMessage)
        {
            this.UIId = UIId;
        }
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
        public int UIId { get; set; }
        public bool Result { get; set; }
        public StudentRegResults Status { get; set; }
        public string ErrorMessage { get; set; }
    }
    public enum StudentRegResults { OK, EmailExists, NoValideModel, Exception, AlreadyOrdered, AlreadyParticipate }
}