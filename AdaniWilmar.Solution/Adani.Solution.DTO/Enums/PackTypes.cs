using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO.Enums
{
    public enum PackTypes
    {
        [Description("Loose")] BulkPacking = 1,
        [Description("Jars")] Jars = 2,
        [Description("Pouches")] Pouches = 3,
        [Description("Tins")] Tins = 4,
        [Description("BIB")] BIB = 5,
        [Description("Bottles")] Bottles = 6,
        [Description("LUPs")] LUPs = 7,
        [Description("Others")] Others = 8,
        [Description("Box")] Box = 9,
        [Description("BAG")] BAG = 10,
        //[Description("PET")] PET = 11
    }
}
