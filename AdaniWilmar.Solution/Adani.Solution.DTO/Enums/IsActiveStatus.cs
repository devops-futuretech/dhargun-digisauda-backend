using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO.Enums
{
    public enum IsActiveStatus
    {
        [Description("All")] All = 1,
        [Description("Active")] Active = 2,
        [Description("InActive")] InActive = 3,
    }
}
