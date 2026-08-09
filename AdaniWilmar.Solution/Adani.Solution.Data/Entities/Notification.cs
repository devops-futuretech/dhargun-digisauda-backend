using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
   public class TPNotification : Auditable
    {
        public bool SMS { get; set; }

        public bool Email { get; set; }

        public bool InAppNotification { get; set; }

     }

}
