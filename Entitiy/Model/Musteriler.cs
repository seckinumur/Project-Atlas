using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entitiy.Model
{
   public class Musteriler
    {
        public int MusterilerID { get; set; }
        public string Isim { get; set; }
        public string Telefon { get; set; }
        public string EMail { get; set; }
        public string Adres { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public double Mesafe { get; set; }
    }
}
