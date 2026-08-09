using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO.Enums
{
    public enum DemoSchedulerStatus
    {
        [Description("Pending")] Pending = 1,
        [Description("Approve for Demo")] ApproveforDemo = 2,
        [Description("Demo not Required")] DemonotRequired = 3,
     }
}
