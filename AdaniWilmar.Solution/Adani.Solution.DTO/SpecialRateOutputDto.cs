using System;
using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class SpecialRateOutputDto
    {
        public long SpecialRateId { get; set; }
        public DateTime RequestDate { get; set; }
        public long DealerId { get; set; }
        public string DealerName { get; set; }
        public long StatusId { get; set; }
        public string StatusName { get; set; }
        public bool IsBroker { get; set; }
        public bool IsLTD { get; set; }
        public decimal SpecialPrice { get; set; }
        public long SkuId { get; set; }
        public string SkuName { get; set; }
        public decimal Quantity { get; set; }
        public decimal DiscountOrPremium { get; set; }
        public long CreatedBy { get; set; }
        public decimal DiscountAmountInConfiguration { get; set; }
        public IList<SpecialRateOilTypeDto> OilTypeList { get; set; }
        public SpecialRateOutputDto()
        {
            OilTypeList = new List<SpecialRateOilTypeDto>();
        }
    }

    public class SpecialRateResultDto
    {
        public long DealerId { get; set; }
        public string DealerName { get; set; }
        public List<SpecialRateOutputDto> SpecialRateList { get; set; }

        public SpecialRateResultDto()
        {
            SpecialRateList = new List<SpecialRateOutputDto>();
        }
    }
}
