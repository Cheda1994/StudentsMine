﻿using System;
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
        public string Email { get; set; }
        public string Name { get; set; }
    }

    public class SudentRegistrationStatus
    {
        public SudentRegistrationStatus(CreateStudentView Student, bool Result, IEnumerable<string> ErrorMessage)
        {
            this.Student = Student;
            this.Result = Result;
            if (Result)
            {
                this.ErrorMessage = "No errors";
            }
            else
            {
                foreach (var message in ErrorMessage)
                {
                    this.ErrorMessage += string.Format("Error: {0}. ", message);
                }
            }
        }
        public CreateStudentView Student { get; set; }
        public bool Result { get; set; }
        public string ErrorMessage { get; set; }
    }
}