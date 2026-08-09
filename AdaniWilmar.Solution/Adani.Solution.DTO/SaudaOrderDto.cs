using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SaudaOrderDto
    {
        public long Id { get; set; }
        public string SaudaNumber { get; set; }
        public long? SkuId { get; set; }
        public decimal BidQuantityCase { get; set; }
        
    }

    public class WeightedAverageDto
    {
        public decimal Weight { get; set; }
        public decimal Price { get; set; }
        public decimal SumOfWeightAndPrice { get; set; }
    }
}
