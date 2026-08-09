using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO.Enums
{
    public enum Uom
    {
        [Description("Car")] Case = 1,
        [Description("Ltr")] Ltr = 2,
        [Description("MT")] MT = 3,
        [Description("Kg")] Kg = 4,
        [Description("NOS")] Nos = 5,
        [Description("Each")] EA = 6,
        [Description("BAG")] BAG = 7,
    }
}
