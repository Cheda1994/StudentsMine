using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using StudentsMine.Models;
using System;
using System.Collections.Generic;
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
        private UserManager<ApplicationUser> UserManager;
        //
        //public StudentController()
        //    : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        //{
        //}

        //public StudentController(UserManager<ApplicationUser> userManager)
        //{
        //    UserManager = userManager;
        //}
        // GET: /Student/
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Teacher")]
        public ActionResult CreateStudent() {
            List<CreateStudentView> students = new List<CreateStudentView>(){
                new CreateStudentView(),
                new CreateStudentView()
            };
            return View(students);
        }

        [HttpPost]
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult> CreateStudent(List<CreateStudentView> model)
        {
            List<SudentRegistrationStatus> results = new List<SudentRegistrationStatus>();
            AccountController account = new AccountController();
            foreach (var item in model)
            {
                var result = await account.RegisterStudent(item);
                results.Add(result);
            }
            var json = new JavaScriptSerializer().Serialize(results);
            return new ContentResult() { 
            Content = json,
            ContentType = "application/json",
            ContentEncoding = Encoding.UTF8
            };
        }
	}
}