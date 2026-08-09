using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO.Enums
{

    public enum TransportMode
    {
        [Description("Truck")] Truck = 1,
        [Description("Rake")] Rake = 2,
        [Description("Lorry")] Lorry = 3,
        [Description("Ven")] Ven = 4
    }
}
