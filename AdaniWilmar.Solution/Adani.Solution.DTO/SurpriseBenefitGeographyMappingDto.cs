using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SurpriseBenefitGeographyMappingDto : IAPIInputDTO
    {
        public long SurpriseBenefitGeographyMappingId { get; set; }
        public long SurpriseBenefitGeographyId { get; set; }

        public long VerticalId { get; set; }
        public string Vertical { get; set; }

        public long OilTypeId { get; set; }
        public string OilTypeName { get; set; }

        public long OilPackingTypeId { get; set; } /* BPOrCPWise - PackGroup - OilPackingTypeId*/
        public string OilPackingType { get; set; }

        public long SkuId { get; set; }
        public string SkuCode { get; set; }
        public string SkuName { get; set; }

        public long StateId { get; set; }
        public string StateName { get; set; }

        public long DistrictId { get; set; }
        public string DistrictName { get; set; }

        public long CityId { get; set; }
        public string CityName { get; set; }

        public long CustomerGroupId { get; set; }
        public string CustomerGroup { get; set; }

        public long CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string UserName { get; set; }
        public string UserCode { get; set; }

        public long SaudaId { get; set; }
        public long SaudaOrderId { get; set; }
        public string SaudaNumber { get; set; }
        public decimal BidQuantityCase { get; set; }
        public decimal BidPrice { get; set; }
        public decimal BidPriceAfterDiscount { get; set; }

        public long BenefitTypeId { get; set; }
        public string BenefitType { get; set; }

        public long BenefitOrCategoryId { get; set; }
        public string BenefitOrCategory { get; set; }

        public long SapBenefitDays { get; set; }
        public decimal NonSapDiscountPerCase { get; set; }
        public decimal NonSapDiscount { get; set; }
        public decimal DiscountOrDays { get; set; }

        public long SaudaValidityPeriod { get; set; }
        public long TotalSaudaValidityDays { get; set; }

        public DateTime BiddingDate { get; set; }
        public DateTime SaudaValidFromDate { get; set; }
        public DateTime SaudaValidToDate { get; set; }
        public DateTime? BenefitAppliedDate { get; set; }
        public DateTime? BenefitModifiedDate { get; set; }

        public bool IsActive { get; set; }
        public long LoginUserId { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }
}
