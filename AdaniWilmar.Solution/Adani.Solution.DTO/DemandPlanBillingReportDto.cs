using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    class DemandPlanBillingReportDto { }

    public class DemandPlanBillingListOutputDto
    {
        public int ListCount { get; set; }
        public List<DemandPlanBillingReportOutputDto> DemandPlanBillingReportOutput { get; set; }

    }
    public class DemandPlanBillingReportInputputDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }

    public class DemandPlanBillingReportOutputDto
    {
        //String
        public string RowLabel { get; set; }

        //Decimal
        public decimal RowLabel1 { get; set; }

        //DateTime
        public DateTime? Date { get; set; }
        


    }
}
