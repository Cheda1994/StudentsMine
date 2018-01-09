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
        public Mark Mark { get; set; }
        public bool IsPublic { get; set; }
        public Student Author { get; set; }
    }
}
