using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO.Enums
{
    public enum DeleteListCreation
    {
        [Description("Dealer inactive Remarks")]
        DealerInactiveRemarks = 1,

        [Description("Ship To Party inactive Remarks")]
        ShipToPartyInactiveRemarks = 2
    }
}
