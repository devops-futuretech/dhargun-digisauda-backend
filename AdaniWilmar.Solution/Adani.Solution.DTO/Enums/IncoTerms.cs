using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO.Enums
{
    public enum IncoTerms
    {
        [Description("For Plant")] ForPlant = 1,
        [Description("Ex Plant")] ExPlant = 2,
        [Description("For Depot")] ForDepot = 3,
        [Description("Ex Depot")] ExDepot = 4,
        [Description("For Rake")] ForRake = 5,
        [Description("Ex Rake")] ExRake = 6
    }
}
