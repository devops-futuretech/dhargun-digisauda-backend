using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO.Enums
{
    public enum LiftingRequestStatus
    {
        [Description("Inprogress")] Inprogress = 1,
        [Description("Confirmed")] Confirmed = 2,
        [Description("Intransist")] Intransist = 3,
       
    }
}
