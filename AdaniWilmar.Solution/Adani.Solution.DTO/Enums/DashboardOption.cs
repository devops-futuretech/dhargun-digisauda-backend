using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO.Enums
{
    public enum DashboardOption
    {
        [Description("TodayContract")] TodayContract = 1,
        [Description("TodaySalesOrder")] TodaySalesOrder = 2,
        [Description("TodayInvoice")] TodayInvoice = 3,
        [Description("Due")] Due = 4
    }
    
}
