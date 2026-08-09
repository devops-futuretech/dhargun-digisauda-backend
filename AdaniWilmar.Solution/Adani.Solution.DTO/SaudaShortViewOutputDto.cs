using System;
using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class SaudaShortViewOutputDto
    {
        public long SaudaId { get; set; }
        public string SaudaNumber { get; set; }
        public DateTime BookedDate { get; set; }
        public DateTime ValidToDate { get; set; }
        public decimal TotalQuantityCases { get; set; }
        public decimal TotalQuantityMT { get; set; }
        public long DealerId { get; set; }
        public string DealerName { get; set; }
        public List<SaudaOrderDetails> SaudaOrders { get; set; }

        public SaudaShortViewOutputDto()
        {
            SaudaOrders = new List<SaudaOrderDetails>();
        }
    }
}
