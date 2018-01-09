using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentsMine.Models
{
    public class OrderToCourse
    {
        public OrderToCourse()
        {
            this.Status = false;
        }
        public int Id { get; set; }
        public bool Status { get; set; }
        public virtual Student Student { get; set; }
        public virtual Course Course { get; set; }
    }
}