using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SaudaReportBasedOnVerticalsDTO
    {
        public List<PlantIdWithMTDto> PlantIdWithMTDto { get; set; }
        public decimal TotalBidQuantity { get; set; }
        public string OilType { get; set; }
        public string Vertical { get; set; }

    }

    public class PlantIdWithMTDto
    {
        public long PlantId { get; set; }
        public decimal BidQuantity { get; set; }
    }
}
