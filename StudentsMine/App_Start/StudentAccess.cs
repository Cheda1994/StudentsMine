﻿using StudentsMine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Security.Principal;
using System.Web.Script.Serialization;
namespace StudentsMine.App_Start
{
    public class StudentAccess :FilterAttribute, IAuthorizationFilter
    {
        public ApplicationDbContext context = new ApplicationDbContext();
        public string accessType;
        public StudentAccess()
        {

        }
        public StudentAccess(string accessType)
        {
            this.accessType = accessType;
        }
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var request = new RequestStatus();
            request.Result = true;
            var userName = System.Security.Claims.ClaimsPrincipal.Current.Identity.Name;
            int courseId = int.Parse(filterContext.HttpContext.Request.QueryString["courseId"]);
            var studentAccess = context.StudentAccessProps.Where(x => x.Course.Id == courseId & x.Student.ApplicationUser.UserName == userName).SingleOrDefault();
            if (!System.Security.Claims.ClaimsPrincipal.Current.IsInRole(ApplicationConstants.TEACHER))
            {

            if (studentAccess.AccessToCourse & accessType != null)
            {
                switch (accessType)
                {
                    case ApplicationConstants.CAN_UPLOAD_FILES:
                        if (!studentAccess.CanUploadFiles)
                        {
                            request.Result = false;
                        }
                        break;
                    case ApplicationConstants.CAN_DOWNLOAD_FILES:
                        if (!studentAccess.CanDownloadFiles)
                        {
                            request.Result = false;
                        }
                        break;
                    case ApplicationConstants.ACCESS_TO_HOMEWORK:
                        if (!studentAccess.AccessToHomeWork)
                        {
                            request.Result = false;
                        }
                        break;
                    case ApplicationConstants.ACCESS_TO_COURSE:
                        if (!studentAccess.AccessToCourse)
                        {
                            request.Result = false;
                        }
                        break;
                    default:
                        break;

                }  
            }
            else
            {
                request.Result = false;
                
            }
                        if (!request.Result)
                        {
                            request.ErrorMessage = "Access to this function is closed by teacher.";
                            var json = new JavaScriptSerializer().Serialize(request);
                            filterContext.Result = new ContentResult() {
                                Content = json,
                                ContentType = ApplicationConstants.JSON_TYPE 
                            };
                        }
        }

        }
    }
}