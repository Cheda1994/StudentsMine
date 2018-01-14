using StudentsMine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Web.Security;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Text;

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

        public ActionResult HomeWorkIndex(int courseId) {
            List<Project> projects = context.Projects.Where(p => p.IsHomeWork==true && p.HomeWork.Course.Id == courseId && p.Author.ApplicationUser.Id == currentUserId).ToList();
            List<HomeWorkListView> hwList = new List<HomeWorkListView>();
            foreach (var project in projects)
            {
                HomeWorkListView homeWork = new HomeWorkListView(project);
                hwList.Add(homeWork);
            }
            return View("~/Views/Cource/HomeWork/Index.cshtml", hwList);
        }

        [HttpGet]
        public async Task<ContentResult> GetHWProject(int projectId)
        {
            Project project = await context.Projects.FindAsync(projectId);  
            if (project.Author.ApplicationUser.Id == currentUserId)
            {
                HomeWorkPresentation homeWork = new HomeWorkPresentation(project);
                var jsonResult = new JavaScriptSerializer().Serialize(homeWork);
                return new ContentResult()
                {
                    Content = jsonResult,
                    ContentType = "application/json",
                    ContentEncoding = Encoding.UTF8
                };
            }
            else
            {
                return new ContentResult()
                {
                    Content = "You have no access to this file",
                    ContentType = "application/json",
                    ContentEncoding = Encoding.UTF8
                };
            }
        }
	}
}