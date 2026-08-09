using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class UserCustomerTarget : Auditable
    {
        public long? AssignedFromId { get; set; }
        public long? AssignedToId { get; set; }
        public int Quarter { get; set; }
        public int MonthId { get; set; }
        public long FinancialYearId { get; set; }
        public long Year { get; set; }
        public decimal Target { get; set; }
        
        public virtual FinancialYear FinancialYear { get; set; }
        public virtual User AssignedFrom { get; set; }
        public virtual User AssignedTo { get; set; }
        public virtual Month Month { get; set; }
    }
}
