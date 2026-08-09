using System;
using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class SaudaShortViewDto
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
        public long SaudaConversionId { get; set; }
        public IList<SpecialRateOilTypeDto> OilTypes { get; set; }
        public SaudaShortViewDto()
        {
            OilTypes = new List<SpecialRateOilTypeDto>();
        }
    }
}
