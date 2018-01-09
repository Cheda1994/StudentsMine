using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentsMine.Models
{
    public class UserTemplate
    {
        public UserTemplate()
        {
            Courses = new List<Course>();
        }
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<Course> Courses { get; set; }


    }
}