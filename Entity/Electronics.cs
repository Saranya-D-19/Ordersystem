﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order_Management_System.Entity
{
    public class Electronics : Product
    {
        public string Brand { get; set; }
        public int WarrantyPeriod { get; set; }
    }
}
