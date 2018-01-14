using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StudentsMine.Models
{
    //TODO: Continue Project
    public enum Mark { ZERO , ONE , TWO , THREE , FOUR , FIVE , SIX , SEVEN , EIGTH , NINE , TEN}
    public class Project
    {
        public Project()
        {
            this.IsPublic = true;
        }
        public int Id { get; set; }
        public int Mark { get; set; }
        public bool IsPublic { get; set; }
        public FileData File { get; set; }
        public virtual Student Author { get; set; }
        public bool IsHomeWork { get; set; }
        public virtual HomeWork HomeWork { get; set; }
    }
}
