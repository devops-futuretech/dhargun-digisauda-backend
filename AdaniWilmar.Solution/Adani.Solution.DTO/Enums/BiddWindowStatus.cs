using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO.Enums
{
    public enum BiddWindowStatus
    {
        [Description("Pending")] Pending = 1,
        [Description("Processing")] Processing = 2,
        [Description("Stopped")] Stopped = 3,
        [Description("Completed")] Completed = 4
    }

    public enum SaudaAllocationStatus
    {
        [Description("Pending")] Pending = 1,
        [Description("Processing")] Processing = 2,
        [Description("Completed")] Completed = 3
    }
}
