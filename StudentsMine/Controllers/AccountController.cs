using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using StudentsMine.Models;
using System.Diagnostics;
using System.Web.Script.Serialization;
using System.Net.Mail;

namespace StudentsMine.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        public AccountController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        public string GetCurrentUserId()
        {
            return User.Identity.GetUserId();
        }
        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            RequestStatus status = new RequestStatus();
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(model.UserName, model.Password);
                if (user != null)
                {
                    await SignInAsync(user, model.RememberMe);
                    status.Result = true;
                    status.ErrorMessage = "No errors";
                    var json = new JavaScriptSerializer().Serialize(status);
                    return new ContentResult()
                    {
                        Content = json,
                        ContentType = "application/json"
                    };
                }
                else
                {
                    status.Result = false;
                    status.ErrorMessage = "Invalid username or password.";
                    var json = new JavaScriptSerializer().Serialize(status);
                    return new ContentResult()
                    {
                        Content = json,
                        ContentType = "application/json"
                    };
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            RegistrationStatus status = model.Verify();
            if (status.Result)
            {
                var user = new ApplicationUser() { UserName = model.UserName };
                user.Teacher = new Teacher();
                user.Role = "Teacher";
                user.Teacher.Name = model.UserName;
                user.Teacher.Email = model.Email;
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var currentUser = UserManager.FindByName(user.UserName);
                    UserManager.AddToRole(currentUser.Id, "Teacher");
                    await SignInAsync(user, isPersistent: false);
                }
                else
                {
                    //AddErrors(result);
                    status.Result = false;
                    foreach (var error in result.Errors)
                    {
                        status.ErrorMessage += error;
                    }
                }
            }
            var json = new JavaScriptSerializer().Serialize(status);

            // If we got this far, something failed, redisplay form
            return new ContentResult()
            {
                Content = json,
                ContentType = "application/json"
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<RegistrationStatus> RegisterStudent(CreateStudentView model)
        {
            RegistrationStatus status = model.Verify();
            try
            {
                if (status.Result)
                {
                    var user = new ApplicationUser() { UserName = model.UserName };
                    user.Student = new Student(model);
                    user.Role = "Student";
                    Guid passowrd = Guid.NewGuid(); // will be password
                    var result = await UserManager.CreateAsync(user, "111111");
                    status = new RegistrationStatus(model, result.Succeeded, StudentRegResults.OK , result.Errors);
                    if (result.Succeeded)
                    {
                        var student = UserManager.FindByName(user.UserName);
                        UserManager.AddToRole(student.Id, "Student");
                        Mailer.Mailer mail = new Mailer.Mailer();
                        mail.SetGeter(user.Student.Email);
                        string emailText = "You was ordet to the course ****** , by *******. <br /> ";
                        emailText += "User name: "+user.UserName+" <br /> ";
                        emailText += "Password: 111111";
                        mail.SetTitleAndBody("Order to Course", emailText);
                        mail.Send();
                    }
                }
            }
            catch (Exception ex)
            {
                status = new RegistrationStatus(model, false, StudentRegResults.Exception, new List<string>() { ex.Message });
            }

            return status;
        }


        //
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage
        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/LogOff

        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //
     

        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {

            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            identity.AddClaims(new[] {
                new Claim("CurrentRole",user.Role)
            });

            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri) : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}