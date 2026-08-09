using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class SAPEmailStatement : Auditable
    {
        public string CompanyName { get; set; }
        public string CustomerName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Currency { get; set; }
        public bool IsWithoutSpecialGL { get; set; }
        public int DocumentType { get; set; }
        public string SAPStatus { get; set; }
        public bool IsActive { get; set; }
    }
}
