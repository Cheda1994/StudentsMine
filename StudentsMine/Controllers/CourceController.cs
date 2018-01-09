using StudentsMine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Web.Security;
using System.Diagnostics;

namespace StudentsMine.Controllers
{
    //TODO : Add exceptions!!!!
    [Authorize]
    public class CourceController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();

        private string currentUserId { 
            get { return User.Identity.GetUserId(); }
        }
        //
        // GET: /Cource/
        public ActionResult Index()
        {
            var currentUser = (ApplicationUser)context.Users.Find(currentUserId);
            ICollection<Course> courses = null;
            switch (currentUser.Role)
            {
                case "Teacher":
                    courses = currentUser.Teacher.Courses;
                    break;
                case "Student":
                    courses = currentUser.Student.Courses;
                    break;
                default:
                    break;
            }
            return View(courses);
        }

        public ActionResult Course(int id) {
            Course course = context.Courses.Find(id);
            ViewBag.CourseId = id;
            return View(course);
        }

        [Authorize(Roles="Teacher")]
        public ActionResult AddCource() {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Teacher")]
        public ActionResult AddCource(Course course)
        {
            Teacher user = context.Teachers.SingleOrDefault(x => x.ApplicationUser.Id == currentUserId);
            course.Teacher = user;
            context.Courses.Add(course);
            context.SaveChanges();
            return View();
        }

        public ActionResult AddStudentsToCource()
        {
            return View();
        }

        public ActionResult AddHomeWork(int courceId) 
        {
            Course cource = context.Courses.Find(courceId);
            HomeWork homeWork = new HomeWork(cource);
            ViewBag.CourceId = courceId;
            return View();
        }

        
        [Authorize(Roles = "Teacher")]
        [HttpPost]
        public ActionResult AddHomeWork(HomeWork homeWork, int courseId)
        {
            Course course = context.Courses.Find(courseId);
            if (course.Teacher.ApplicationUser.Id == currentUserId)
            {
                homeWork.InitProjects(course);
                context.Homeworks.Add(homeWork);
                context.SaveChanges();
            }
            return View();
        }
	}
}