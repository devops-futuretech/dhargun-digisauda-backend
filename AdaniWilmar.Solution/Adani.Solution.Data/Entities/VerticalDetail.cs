using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
   public class DivisionDetail : Auditable
    {
        public int DivisionId { get; set; }
        public string CCArea { get; set; }
    }
}
