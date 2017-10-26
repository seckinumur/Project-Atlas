using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entitiy.Model
{
   public class Nav
    {
        public int NavID { get; set; }
        public int MusterilerID { get; set; }

        public virtual Musteriler Musteriler { get; set; }
    }
}
