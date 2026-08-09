using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
   public class ConsentImageDetailsForCustomers : Auditable
    {
        public long UserId { get; set; }
        public string FileName { get; set; }
        public string MediaPath { get; set; }
        public int? MediaTypeId { get; set; }
        public virtual MediaType MediaType { get; set; }
    }
}
