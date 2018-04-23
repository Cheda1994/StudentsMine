using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentsMine.App_Start
{
    public static class ApplicationConstants
    {
        // Roles

        public  const string TEACHER = "Teacher";
        public  const string STUDENT = "Student";

        // End Roles

        // Student Access 

        public  const string CAN_UPLOAD_FILES = "CanUploadFiles";
        public  const string ACCESS_TO_HOMEWORK = "AccessToHomeWork";
        public  const string CAN_DOWNLOAD_FILES = "CanDownloadFiles";
        public  const string ACCESS_TO_COURSE = "AccessToCourse";

        // End Student Access

        // Content types

        public  const string JSON_TYPE = "application/json";

        // End Content types
    }
}