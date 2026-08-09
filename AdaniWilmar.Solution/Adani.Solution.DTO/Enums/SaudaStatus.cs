using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO.Enums
{
   
    public enum SaudaStatus
    {
        [Description("Not Released")] NotReleased = 1,
        [Description("Released")] Released = 2,
        [Description("Open")] Open = 3,
        [Description("Blocked")] Blocked = 4,
        [Description("Processed")] Processed = 5
    }
}
