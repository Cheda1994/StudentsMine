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
using System.Security.Claims;

namespace StudentsMine.Controllers
{
    //TODO : Add exceptions!!!!
    [Authorize]
    public class CourceController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();

        private string currentUserId { 
            get {
                return User.Identity.GetUserId(); }
        }

        private string currentUserRole
        {
            get
            {
                string roleNames = ((ClaimsIdentity)User.Identity).FindFirst("CurrentRole").Value;
                return roleNames;
            }
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
            return View(homeWork);
        }

        
        [Authorize(Roles = "Teacher")]
        [HttpPost]
        public ContentResult AddHomeWork(HomeWork homeWork, int courseId)
        {
            RequestStatus result = new RequestStatus();
            try
            {
                Course course = context.Courses.Find(courseId);
                if (course.Teacher.ApplicationUser.Id == currentUserId)
                {
                    homeWork.InitProjects(course);
                    if (homeWork.Attachments != null)
                    {
                        FileData.DataBase64ToDataByteArr(homeWork.Attachments);
                        FileData.BuindGuid(homeWork.Attachments);
                    }
                    else
                    {
                        throw new NullReferenceException();
                    }
                    context.Homeworks.Add(homeWork);
                    context.SaveChanges();
                    result.Result = true;
                    result.ErrorMessage = "No errors";
                }
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.ErrorMessage = ex.Message;
            }
            var json = new JavaScriptSerializer().Serialize(result);
            return new ContentResult() { 
                Content = json,
                ContentType = "application/json"
            };
        }

        public ActionResult HomeWorkIndex(int courseId) {
            switch (currentUserRole)
            {
                case "Teacher":
                    return TeacherHomeWorkIndex(courseId);
                case "Student":
                    return StudentHomeWorkIndex(courseId);
                default:
                    return View();
            }
            
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
                    if (condition.HasRequiredFormat)
                    {
                        if (condition.RequiredFormat == model.Format)
                        {
                            return new UploadHomeWorkResult(true, UploadHomeWorkStatus.OK, "No errors");
                        }
                        else
                        {
                            return new UploadHomeWorkResult(false, UploadHomeWorkStatus.IncorrectFormat, "There is incorect file format");
                        }
                    }
                    else
                    {
                        return new UploadHomeWorkResult(true, UploadHomeWorkStatus.OK, "No errors");
                    }
                }
                else
                {
                    return new UploadHomeWorkResult(true, UploadHomeWorkStatus.OK, "No errors");
                }
	        }
            else
	        {
                return new UploadHomeWorkResult(false, UploadHomeWorkStatus.IncorrectFormat, "There is incorect project Id");
	        }
            }
            else 
            {
                return new UploadHomeWorkResult(false, UploadHomeWorkStatus.Exception, "Exception"); 
            }
        }

        private ActionResult StudentHomeWorkIndex(int courseId)
        {
            List<Project> projects = context.Projects.Where(p => p.IsHomeWork == true && p.HomeWork.Course.Id == courseId && p.Author.ApplicationUser.Id == currentUserId).ToList();
            List<HomeWorkStudentListView> hwList = new List<HomeWorkStudentListView>();
            foreach (var project in projects)
            {
                HomeWorkStudentListView homeWork = new HomeWorkStudentListView(project);
                hwList.Add(homeWork);
            }
            return View("~/Views/Cource/HomeWork/Index.cshtml", hwList);
        }

        [Authorize(Roles = "Teacher")]
        private ActionResult TeacherHomeWorkIndex(int courseId)
        {
            context.Configuration.LazyLoadingEnabled = false;
            IQueryable<HomeWork> homeWork = context.Homeworks.Where(hw => hw.Course.Id == courseId).Include("Condition");
            return View("~/Views/Cource/HomeWork/TeacherHWIndex.cshtml", homeWork);
        }
        #endregion

        #region Home Work options
        [Authorize(Roles = "Teacher")]
        public async Task<ContentResult> BlockHomeWork(int homeworkId)
        {
            RequestStatus status = new RequestStatus();
            var homeWork = await context.Homeworks.FindAsync(homeworkId);
            if (homeWork != null)
            {
                if (homeWork.Course.Teacher.ApplicationUser.Id == currentUserId)
                {
                    if (homeWork.HasCondition)
                    {
                        homeWork.Condition.IsBlocked = !homeWork.Condition.IsBlocked;
                    }
                    else
                    {
                        homeWork.Condition = new Condition();
                        homeWork.Condition.IsBlocked = true;
                    }
                    homeWork.HasCondition = true;
                    context.Entry(homeWork).State = EntityState.Modified;
                    context.SaveChanges();
                    status.Result = true;
                    status.ErrorMessage = "No errors";
                }
                else
                {
                    status.Result = false;
                    status.ErrorMessage = "The ID of homework is incorrect";
                }
            }
            else
            {
                status.Result = false;
                status.ErrorMessage = "The HW is not define in the DB";
            }
            var json = new JavaScriptSerializer().Serialize(status);
            return new ContentResult()
            {
                Content = json,
                ContentType = "application/json",
                ContentEncoding = Encoding.UTF8
            };
        }

        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult> GetHWProjectsList(int homeworkId)
        {
            HomeWorkProjectsList projects = new HomeWorkProjectsList();
            var homeWork = await context.Homeworks.FindAsync(homeworkId);
            if (homeWork != null)
            {
                if (homeWork.Course.Teacher.ApplicationUser.Id == currentUserId)
                {
                    foreach (var project in homeWork.Projects)
                    {
                        projects.Projects.Add(new HomeWorkProject(project));
                    }
                    projects.Result = true;
                    projects.ErrorMessage = "No errors";
                    projects.Status = HomeWorkStatus.OK;
                }
                else
                {
                    projects.Result = false;
                    projects.ErrorMessage = "The ID of homework is incorrect";
                    projects.Status = HomeWorkStatus.IncorrectHWID;
                }
            }
            else
            {
                projects.Result = false;
                projects.ErrorMessage = "The HW is not define in the DB";
                projects.Status = HomeWorkStatus.CannotFindHW;
            }
            var json = new JavaScriptSerializer().Serialize(projects);
            return new ContentResult()
            {
                Content = json,
                ContentType = "application/json",
                ContentEncoding = Encoding.UTF8
            };
        }

        public async Task<ContentResult> RemoveHW(int homeworkId)
        {
            RequestStatus status = new RequestStatus();
            var homeWork = await context.Homeworks.FindAsync(homeworkId);
            if (homeWork != null)
            {
                if (homeWork.Course.Teacher.ApplicationUser.Id == currentUserId)
                {
                    context.Homeworks.Remove(homeWork);
                    context.SaveChanges();
                    status.Result = true;
                    status.ErrorMessage = "No errors";
                }
                else
                {
                    status.Result = false;
                    status.ErrorMessage = "The ID of homework is incorrect";
                }
            }
            else
            {
                status.Result = false;
                status.ErrorMessage = "The HW is not define in the DB";
            }
            var json = new JavaScriptSerializer().Serialize(status);
            return new ContentResult()
            {
                Content = json,
                ContentType = "application/json",
                ContentEncoding = Encoding.UTF8
            };
        }

        [Authorize(Roles = "Teacher")]
        public async Task<ContentResult> EditHW(Condition condition, int homeworkId)
        {
            return new ContentResult();
        }
        #endregion

        #region Projects options
        [Authorize(Roles = "Teacher")]
        public ContentResult GetFile(int projectId)
        {
            var project = context.Projects.Include("File").Where(x => x.Id == projectId).SingleOrDefault();
            if (project.IsUploaded)
            {
                if (project.IsPublic)
                {
                    var file = project.File;
                    var base64 = Convert.ToBase64String(file.Data);
                    var format = file.Format;
                    var name = file.Name;
                    return new ContentResult()
                    {
                        Content = format + "," + base64
                    };
                }
                else if (project.Author.ApplicationUser.Id == currentUserId | project.HomeWork.Course.Teacher.ApplicationUser.Id == currentUserId)
                {
                    var file = project.File;
                    var base64 = Convert.ToBase64String(file.Data);
                    var format = file.Format;
                    var name = file.Name;
                    return new ContentResult()
                    {
                        Content = format + "," + base64
                    };
                }
                else
                {
                    return new ContentResult()
                    {
                        Content = "You have no access"
                    };
                }
            }
            else
            {
                return new ContentResult()
                {
                    Content = "The project was no uploaded"
                };
            }
        }

        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult> ChangeMark(int projectId, int mark)
        {
            RequestStatus result;
            var project = await context.Projects.FindAsync(projectId);
            if (project != null)
            {
                if (project.IsHomeWork)
                {
                    if (project.HomeWork.Course.Teacher.ApplicationUser.Id == currentUserId)
                    {
                        project.Mark = mark;
                        context.Entry(project).State = EntityState.Modified;
                        context.SaveChanges();
                        result = new RequestStatus();
                        result.Result = true;
                        result.ErrorMessage = "No Errors";
                    }
                    else
                    {
                        result = new RequestStatus();
                        result.Result = false;
                        result.ErrorMessage = "The project is available for you";
                    }
                }
                else
                {
                    result = new RequestStatus();
                    result.Result = false;
                    result.ErrorMessage = "The project is not home work";
                }
            }
            else
            {
                result = new RequestStatus();
                result.Result = false;
                result.ErrorMessage = "The project is not found";
            }
            var json = new JavaScriptSerializer().Serialize(result);
            return new ContentResult()
            {
                Content = json,
                ContentType = "application/json"
            };
        }

        [HttpGet]
        public async Task<ContentResult> GetHWProject(int projectId)
        {
            Project project = context.Projects.Include(p => p.HomeWork.Attachments).Where(p => p.Id == projectId).SingleOrDefault();
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
            Project project = await context.Projects.FindAsync(model.ProjectId);
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
                    file.Guid = Guid.NewGuid();
                    file.Name = model.Name;
                    file.Format = model.Format;
                    byte[] formatedFile = Convert.FromBase64String(formatedString);
                    file.Data = formatedFile;
                    project.File = file;
                    project.IsUploaded = true;
                    context.Entry(project).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
            var json = new JavaScriptSerializer().Serialize(projectValidation);
            return new ContentResult()
            {
                Content = json,
                ContentType = "application/json",
                ContentEncoding = Encoding.UTF8
            };
        }


        public ContentResult GetHWAttachment(string fileId) {
            FileData file = context.Files.Where(f => f.Guid.ToString() == fileId).SingleOrDefault();
            string json;
            if (file == null)
            {
                json = new JavaScriptSerializer().Serialize("EXCEPTION");
            }
            else
            {
                file.DataBase64 = Convert.ToBase64String(file.Data);
                json = new JavaScriptSerializer().Serialize(file);
            }
            return new ContentResult()
            {
                Content = json,
                ContentType = "application/json",
                ContentEncoding = Encoding.UTF8
            };
        } 
        #endregion



        public ActionResult ManageStudents(int courseId) {
            ViewBag.CourseId = courseId;
            return View();
        }

        public PartialViewResult EditHomeWork()
        {
            return PartialView("~/Views/Cource/HomeWork/_EditHomeWork.cshtml");
        }

        [Authorize(Roles = "Teacher")]
        [HttpGet]
        public ContentResult GetAllStudents(int courseId) {
            List<Student> students = context.Courses.Find(courseId).Students.ToList();
            if (students != null)
            {
                List<StudentShortView> studentViews = new List<StudentShortView>(); 
                foreach (var student in students)
                {
                    studentViews.Add(new StudentShortView(student));
                }
                var json = new JavaScriptSerializer().Serialize(studentViews);
                return new ContentResult()
                {
                    Content = json,
                    ContentType = "application/json"
                };
            }
            else
            {
                var result = new RequestStatus()
                {
                    ErrorMessage = "Have no find students",
                    Result = false
                };
                var json = new JavaScriptSerializer().Serialize(result);
                return new ContentResult()
                {
                    Content = json,
                    ContentType = "application/json"
                };
            }
            
        }

        [Authorize(Roles = "Teacher")]
        [HttpGet]
        public ContentResult GetAllOrders(int courseId) {
            IQueryable<OrderToCourse> orders = context.OrdersToCourse.Where(x => x.Course.Id == courseId & x.Status == false);
            if (orders != null)
            {
                List<StudentShortView> studentViews = new List<StudentShortView>(); 
                foreach (var order in orders.ToList())
                {
                    studentViews.Add(new StudentShortView(order.Student));
                }
                var json = new JavaScriptSerializer().Serialize(studentViews);
                return new ContentResult()
                {
                    Content = json,
                    ContentType = "application/json"
                };
            }
            return null;
        }

        [Authorize(Roles = "Student")]
        [HttpGet]
        public ContentResult OrdersToCource() 
        {
            List<OrderToCourse> orders = context.OrdersToCourse.Where(x => x.Student.ApplicationUser.Id == currentUserId & x.Status == false).ToList();
            if (orders != null)
            {
                List<OrderToCourceStudentView> ordersView = new List<OrderToCourceStudentView>();
                foreach (var order in orders)
                {
                    ordersView.Add(new OrderToCourceStudentView(order));
                }
                var json = new JavaScriptSerializer().Serialize(ordersView);
                return new ContentResult()
                {
                    Content = json,
                    ContentType = "application/json"
                };
            }
            return null;
        }

        [HttpPost]
        public ContentResult SuccessToOrder(int orderId)
        {
            OrderToCourse order = context.OrdersToCourse.Find(orderId);
            RequestStatus status = new RequestStatus();
            if (order != null)
            {
                order.Status = true;
                order.Course.Students.Add(order.Student);
                context.Entry(order).State = EntityState.Modified;
                context.SaveChanges();
                status.Result = true;
                status.ErrorMessage = "No errors";

            }
            else
            {
                status.Result = false;
                status.ErrorMessage = "Cannod find order";
            }
            var json = new JavaScriptSerializer().Serialize(status);
            return new ContentResult()
            {
                Content=json,
                ContentType="application/json"
            };
        }
    }
}