using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO.Enums
{
    public enum BenefitType
    {
        [Description("SAP")] SAP = 1,
        [Description("NONSAP")] NONSAP = 2,
    }
}
