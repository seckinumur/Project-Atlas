﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.VM
{
   public class VMCore
    {
        public int CoreID { get; set; }
        public string FirmaAdi { get; set; }
        public string Adres { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public string Sifre { get; set; }
    }
}
