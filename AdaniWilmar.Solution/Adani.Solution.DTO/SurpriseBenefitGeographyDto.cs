using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SurpriseBenefitGeographyDto : IAPIInputDTO
    {
        public long Id { get; set; }
        public long VerticalId { get; set; }
        public List<long> OilTypeIds { get; set; }
        public List<long> OilPackingTypeIds { get; set; } /* BPOrCPWise - PackGroup - OilPackingTypeId*/
        public List<long> CustomerGroupIds { get; set; }
        public List<long> CustomerIds { get; set; }
        public List<long> SkuIds { get; set; }
        public List<int> CityIds { get; set; }
        public List<long> ZoneIds { get; set; }
        public List<int> StateIds { get; set; }
        public List<int> DistrictIds { get; set; }
        public List<int> TerritoryIds { get; set; }

        public decimal DiscountOrDays { get; set; }
        public long PercentileNumber { get; set; }

        public List<long> BenefitsIds { get; set; }
        public long BenefitTypeId { get; set; }
        public long BenefitOrCategoryId { get; set; }
        public string BenefitType { get; set; }
        public string BenefitOrCategory { get; set; }

        public decimal SapDays { get; set; }
        public decimal NonSapDiscount { get; set; }

        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public long LoginUserId { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }

        public List<SurpriseBenefitGeographyMappingDto> SurpriseBenefitGeographyMappingDto { get; set; }

        public SurpriseBenefitGeographyDto()
        {
            SurpriseBenefitGeographyMappingDto = new List<SurpriseBenefitGeographyMappingDto>();
        }
    }
}
