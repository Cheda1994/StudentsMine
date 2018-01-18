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
using System.Data.Entity;

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

        [HttpPost]
        public async Task<ActionResult> UploadHomeWork(UploadHomeWork model)
        {
            Project project =  await context.Projects.FindAsync(model.ProjectId);
            UploadHomeWorkResult projectValidation = CheckForHWConditions(project, model);
                if (projectValidation.Result)
                {
                    string formatedString = FileData.FormatBase64(model.FileBase64Data);
                    if (formatedString == "Exception")
                    {
                        projectValidation = new UploadHomeWorkResult(false, UploadHomeWorkStatus.Exception, "There is incorect file");
                    }
                    else 
                    {
                        FileData file = new FileData();
                        file.Name = model.Name;
                        file.Format = model.Format;
                        byte[] formatedFile = Convert.FromBase64String(formatedString);
                        file.Data = formatedFile;
                        project.File = file;
                        context.Entry(project).State = EntityState.Modified;
                        context.SaveChanges();
                    }
                }
                var json = new JavaScriptSerializer().Serialize(projectValidation);
                return new ContentResult()
                {
                    Content = json,
                    ContentType = "application/json"
                };
        }

        #region Private Helpers

        private UploadHomeWorkResult CheckForHWConditions(Project project, UploadHomeWork model)
        {
            if (project != null)
            {
            if (project.IsHomeWork)
	        {
		    HomeWork homeWork = project.HomeWork;
                if (homeWork.HasCondition)
                {
                    Condition condition = homeWork.Condition;
                    if (condition.IsBlocked)
                    {
                        return new UploadHomeWorkResult(false , UploadHomeWorkStatus.AlreadyBloked , "This home work is already blocked");
                    }
                    else
                    {
                        if (condition.HasRequiredDate)
                        {
                            DateTime date = DateTime.Now;
                            if (date != condition.Until)
                            {
                                return new UploadHomeWorkResult(false, UploadHomeWorkStatus.AlreadyBloked, "This home work is already blocked");
                            }
                        }
                    }
                    if (condition.RequiredFormat == model.Format)
                    {
                        return new UploadHomeWorkResult(true, UploadHomeWorkStatus.OK, "No errors");
                    }
                    else
                    {
                        return new UploadHomeWorkResult(false, UploadHomeWorkStatus.IncorectFormat, "There is incorect file format");
                    }
                }
                else
                {
                    return new UploadHomeWorkResult(true, UploadHomeWorkStatus.OK, "No errors");
                }
	        }
            else
	        {
                return new UploadHomeWorkResult(false, UploadHomeWorkStatus.IncorectFormat, "There is incorect project Id");
	        }
            }
            else 
            {
                return new UploadHomeWorkResult(false, UploadHomeWorkStatus.Exception, "Exception"); 
            }
        }
        #endregion
    }
}