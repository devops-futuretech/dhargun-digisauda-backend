using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO.Enums
{
    public enum CustomerGroup
    {
        [Description("01")] Customer = 01,
        [Description("02")] Broker = 02,
        [Description("10")] ModernTrade = 10,
    }
}
