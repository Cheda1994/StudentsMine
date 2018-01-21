using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StudentsMine.Models
{
    //TODO: Continue Project
    public class Project
    {
        public Project()
        {
            this.IsPublic = true;
            this.IsUploaded = false;
        }
        public int Id { get; set; }
        public int Mark { get; set; }
        public bool IsPublic { get; set; }
        public FileData File { get; set; }
        public virtual Student Author { get; set; }
        public bool IsHomeWork { get; set; }
        public bool IsUploaded { get; set; }
        public virtual HomeWork HomeWork { get; set; }
    }
}
