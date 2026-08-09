using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO.Enums
{
    public enum CreditId
    {
        [Description("CreditLimit")] CreditLimit = 1,
        [Description("CreditExposure")] CreditExposure = 2
    }
}
