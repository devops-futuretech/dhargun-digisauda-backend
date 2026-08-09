using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO.Enums
{
    public enum MonthlyReport
    {
        [Description("Contract Report")] SaudaReport = 1,
        [Description("Sales Order Request")] LiftingRequest = 2,
        [Description("Invoice Report")] InvoiceReport = 3
    }
}