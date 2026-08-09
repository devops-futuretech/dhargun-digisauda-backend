using Adani.Solution.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class UserCustomerSalesTarget : Auditable
    {
        public long? AssignedFromId { get; set; }
        public long? AssignedToId { get; set; }
        public int Quarter { get; set; }
        public int MonthId { get; set; }
        public long FinancialYearId { get; set; }
        public long Year { get; set; }
        public long? DivisionId { get; set; }
        public long? SalesOrganizationId { get; set; }
        public long? DistributionChannelId { get; set; }
        public long OilTypeId { get; set; }

        [DecimalPrecision(18, 4)]
        public decimal Target { get; set; }

        //public virtual User Dealer { get; set; }
        public virtual FinancialYear FinancialYear { get; set; }
        public virtual User AssignedFrom { get; set; }
        public virtual User AssignedTo { get; set; }
        public virtual Month Month { get; set; }
        public virtual Division Division { get; set; }
        public virtual OilType OilType { get; set; }
    }
}
