﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StudentsMine.Models
{
    public class Condition
    {
        public Condition()
        {
            this.IsBlocked = false;
        }
        public int Id { get; set; }
        public bool HasRequiredDate { get; set; }
        public DateTime? Until { get; set; }
        public bool IsBlocked { get; set; }
        public bool HasRequiredFormat { get; set; }
        public string RequiredFormat { get; set; }

    }
}
