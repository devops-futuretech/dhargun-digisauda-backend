using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class DailyRateOutputDto
    {
        public string SkuName { get; set; }
        public long SkuId { get; set; }
        public long PlantDepotId { get; set; }
        public string PlantDepotName { get; set; }
        public decimal FinalPrice { get; set; }
    }
}
