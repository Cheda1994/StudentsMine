using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StudentsMine.Models
{
    public class StudentAccessProp
    {
        public int Id { get; set; }
        public Student Student { get; set; }
        public Course Course { get; set; }
        public bool AccessToCourse { get; set; }
        public bool CanUploadFiles { get; set; }
        public bool CanDownloadFiles { get; set; }
        public bool AccessToHomeWork { get; set; }

        public StudentAccessProp MakeClone()
        {
            var clone = new StudentAccessProp() { 
                AccessToCourse = this.AccessToCourse,
                CanDownloadFiles = this.CanDownloadFiles,
                CanUploadFiles = this.CanUploadFiles,
                AccessToHomeWork = this.AccessToHomeWork
            };
            return clone;
        }

        public void UpdateByView(StudentAccessPropView access) {
            this.AccessToCourse = access.AccessToCourse;
            this.CanUploadFiles = access.CanUploadFiles;
            this.CanDownloadFiles = access.CanDownloadFiles;
            this.AccessToHomeWork = access.AccessToHomeWork;
        }
    }

    public class StudentAccessPropView
    {
        public bool AccessToCourse { get; set; }
        public bool CanUploadFiles { get; set; }
        public bool CanDownloadFiles { get; set; }
        public bool AccessToHomeWork { get; set; }
    }

   
}