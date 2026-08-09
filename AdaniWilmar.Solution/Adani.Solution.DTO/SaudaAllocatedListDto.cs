using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SaudaAllocatedListDto
    {
        public long SaudaId { get; set; }
        public long SaudaOrderId { get; set; }
        public string SaudaNumber { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime BookedDate { get; set; }
        public long? StatusId { get; set; }
        public string StatusName { get; set; }
        public long DealerId { get; set; }
        public string DealerName { get; set; }
        public decimal BidQuantity { get; set; }
        public decimal BidQuantityCase { get; set; }
        public long OilTypeId { get; set; }
        public string OilTypeName { get; set; }
        public long SkuId { get; set; }
        public string SkuName { get; set; }
    }
}
