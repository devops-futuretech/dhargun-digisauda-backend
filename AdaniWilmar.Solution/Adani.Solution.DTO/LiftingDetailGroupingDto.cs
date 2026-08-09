using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class LiftingDetailGroupingDto
    {
        public string OilType { get; set; }
        public long NumberOfSKU { get; set; }
        public string SKUName { get; set; }
        public decimal TotalQty { get; set; }
        public decimal LiftedQty { get; set; }
        public decimal PendingQty { get; set; }
    }
}
