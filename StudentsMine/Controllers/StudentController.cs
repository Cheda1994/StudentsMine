using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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

        [Authorize(Roles = "Teacher")]
        public ActionResult CreateStudent(int id) {
            ViewData["courseId"] = id;
            List<CreateStudentView> students = new List<CreateStudentView>(){
                new CreateStudentView(),
                new CreateStudentView()
            };
            return View(students);
        }

        [HttpPost]
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult> CreateStudent(List<CreateStudentView> model , int courseId)
        {
            List<SudentRegistrationStatus> results = new List<SudentRegistrationStatus>();
            AccountController account = new AccountController();
            try
            {
                foreach (var item in model)
                {
                    if (EmailAlreadyExists(item))
                    {
                        var result = await account.RegisterStudent(item);
                        OrderToCourse order = new OrderToCourse();
                        order.Student = context.Students.FirstOrDefault(student => student.Email ==item.Email );
                        order.Course = context.Courses.Find(courseId);
                        context.OrdersToCourse.Add(order);
                        context.SaveChanges();
                        results.Add(result);
                    }
                    else if(IsStudent(item))
                    {
                        if (AlreadyHasOrder(courseId , item))
                        {
                            results.Add(new SudentRegistrationStatus(item, false, StudentRegStatus.AlreadyOrdered , new List<string>() { "This email is already orderd" }));
                        }
                        else
                        {
                            results.Add(new SudentRegistrationStatus(item, false, StudentRegStatus.ExistsEmail, new List<string>() { "This email is already exists" }));
                        }
                    }
                }
                var json = new JavaScriptSerializer().Serialize(results);
                return new ContentResult()
                {
                    Content = json,
                    ContentType = "application/json",
                    ContentEncoding = Encoding.UTF8
                };
            }
            catch (Exception ex)
            {

                return new ContentResult()
                {
                    Content = ex.Message,
                    ContentType = "application/json",
                    ContentEncoding = Encoding.UTF8
                };
            }

        }

        public async Task<ActionResult> SendOrderToCourse(string email,int courseId) {
            try
            {
                Student sturent = context.Students.FirstOrDefault(x => x.Email == email);
                if (sturent == null)
                {
                    return new ContentResult()
                {
                    Content = "The email "+email+" is not available",
                    ContentType = "application/json",
                    ContentEncoding = Encoding.UTF8
                };
                }
                Course course = context.Courses.Find(courseId);
                if (course.Students.Contains(sturent))
                {
                    return new ContentResult()
                {
                    Content = "The email "+email+" is already student in this course",
                    ContentType = "application/json",
                    ContentEncoding = Encoding.UTF8
                };
                }
                course.Students.Contains(sturent);
                course.Students.Add(sturent);
                context.Entry<Course>(course).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return new ContentResult()
                {
                    Content = "Success",
                    ContentType = "application/json",
                    ContentEncoding = Encoding.UTF8
                };
            }
            catch (Exception ex)
            {
                return new ContentResult()
                {
                    Content = ex.Message,
                    ContentType = "application/json",
                    ContentEncoding = Encoding.UTF8
                };
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

        private bool IsStudent(CreateStudentView model)
        {
            int teachers = context.Teachers.Where(x => x.Email == model.Email).Count();
            if (teachers != 0)
            {
                return false;
            }
            return true;
        }

        private bool AlreadyHasOrder(int courceId , CreateStudentView model) 
        {
            var orders = context.OrdersToCourse.Where(x => x.Course.Id == courceId && x.Student.Email == model.Email).Count();
            if (orders == 0)
            {
                return false;
            }
            return true;
        }
        #endregion
    }
}