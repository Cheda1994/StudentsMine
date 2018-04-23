using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using StudentsMine.App_Start;
using StudentsMine.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace StudentsMine.Controllers
{
    
    public class StudentController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = ApplicationConstants.TEACHER)]
        public ActionResult CreateStudent(int id) {
            ViewData["courseId"] = id;
            List<CreateStudentView> students = new List<CreateStudentView>(){
                new CreateStudentView(),
                new CreateStudentView()
            };
            return View(students);
        }

        [HttpPost]
        [Authorize(Roles = ApplicationConstants.TEACHER)]
        public async Task<ActionResult> CreateStudent(List<CreateStudentView> model , int courseId)
        {
            List<RegistrationStatus> results = new List<RegistrationStatus>();
            AccountController account = new AccountController();
            try
            {
                foreach (var item in model)
                {
                    if (EmailAlreadyExists(item))
                    {
                        var result = await account.RegisterStudent(item);
                        if (result.Result)
                        {
                            await SendOrderToCourse(item.Email, courseId);
                        }
                        results.Add(result);
                    }
                    else if(IsStudent(item.Email))
                    {
                        if (AlreadyHasOrder(courseId , item.Email))
                        {
                            results.Add(new RegistrationStatus(item, false, StudentRegResults.AlreadyOrdered, new List<string>() { "This email " + item.Email + " is already orderd." }));
                        }
                        else
                        {
                            results.Add(new RegistrationStatus(item, false, StudentRegResults.EmailExists, new List<string>() { "This email "+ item.Email +" is already exists" }));
                        }
                    }
                    else
                    {
                        results.Add(new RegistrationStatus(item, false, StudentRegResults.Exception, new List<string>() { "This email " + item.Email + " is already exists and cannot be added to youre course." }));
                    }
                }
                var json = new JavaScriptSerializer().Serialize(results);
                return new ContentResult()
                {
                    Content = json,
                    ContentType = ApplicationConstants.JSON_TYPE,
                    ContentEncoding = Encoding.UTF8
                };
            }
            catch (Exception ex)
            {

                return new ContentResult()
                {
                    Content = ex.Message,
                    ContentType = ApplicationConstants.JSON_TYPE,
                    ContentEncoding = Encoding.UTF8
                };
            }

        }

        
        public ActionResult AddStudentToCourse(int id)
        {
            ViewData["courseId"] = id;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = ApplicationConstants.TEACHER)]
        public async Task<ActionResult> AddStudentToCourse(List<OrderStudentToCourceView> email , int courseId) {
            List<SudentAddToCourseStatus> results = new List<SudentAddToCourseStatus>();
            foreach (var item in email)
            {
                SudentAddToCourseStatus result;
                if (IsStudent(item.Email))
                {
                    result = await SendOrderToCourse(item.Email, courseId);            
                }
                else
                {
                    result = new SudentAddToCourseStatus(item.Email, false, StudentRegResults.Exception , new List<string> { "The email " + email + " is email of ticher" });
                }
                results.Add(result);
            }
            var json = new JavaScriptSerializer().Serialize(results);
            return new ContentResult()
            {
                Content = json,
                ContentType = ApplicationConstants.JSON_TYPE,
                ContentEncoding = Encoding.UTF8
            };
        }

        public async Task<SudentAddToCourseStatus> SendOrderToCourse(string email, int courseId)
        {
            try
            {
                Student sturent = context.Students.FirstOrDefault(x => x.Email == email);
                if (sturent == null)
                {
                  return new SudentAddToCourseStatus(email, false, StudentRegResults.Exception, new List<string> { "The email " + email + " is not available" });
                }
                Course course = context.Courses.Find(courseId);
                if (course.Students.Contains(sturent))
                {
                    return new SudentAddToCourseStatus(email, false, StudentRegResults.AlreadyParticipate , new List<string> { "The email " + email + " is already student in this course" });
                }
                if (AlreadyHasOrder(courseId,email))
                {
                    return new SudentAddToCourseStatus(email, false, StudentRegResults.AlreadyOrdered , new List<string> { "This email "+email+" is already orderd" });
                }
                OrderToCourse order = new OrderToCourse(course,sturent);
                context.OrdersToCourse.Add(order);
                await context.SaveChangesAsync();
                return new SudentAddToCourseStatus(email, false, StudentRegResults.OK, new List<string> { "No errors" });
            }
            catch (Exception ex)
            {
                return new SudentAddToCourseStatus(email, false, StudentRegResults.Exception, new List<string> { ex.Message });
            }
        }





        #region Private Helpers
        private bool EmailAlreadyExists(CreateStudentView model) {
            int students = context.Students.Where(x => x.Email == model.Email).Count();
            if (students != 0)
            {
                return false;
            }
            return true;
        }

        private bool IsStudent(string studentEmail)
        {
            int teachers = context.Teachers.Where(x => x.Email == studentEmail).Count();
            if (teachers != 0)
            {
                return false;
            }
            return true;
        }

        private bool AlreadyHasOrder(int courceId , string studentEmail) 
        {
            var orders = context.OrdersToCourse.Where(x => x.Course.Id == courceId && x.Student.Email == studentEmail).Count();
            if (orders == 0)
            {
                return false;
            }
            return true;
        }
        #endregion
    }
}