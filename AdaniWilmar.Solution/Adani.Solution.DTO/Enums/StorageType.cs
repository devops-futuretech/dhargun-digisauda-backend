using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO.Enums
{
    public enum StorageType
    {
        [Description("Plant")] Plant = 1,
        [Description("Depot")] Depot = 2,
        [Description("Rake")] Rake = 3,
    }
}
