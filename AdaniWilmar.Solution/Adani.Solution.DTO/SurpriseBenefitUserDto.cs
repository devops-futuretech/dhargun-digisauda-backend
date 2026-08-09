using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SurpriseBenefitUserDto : IAPIInputDTO
    {
        public long Id { get; set; }
        public long VerticalId { get; set; }
        public List<long> OilTypeIds { get; set; }
        public List<long> OilPackingTypeIds { get; set; }  /* BPOrCPWise - PackGroup - OilPackingTypeId*/
        public List<long> CustomerGroupIds { get; set; }
        public List<long> CustomerIds { get; set; }
        public List<long> ZonalHeadIds { get; set; }
        public List<long> BDOIds { get; set; }
        public List<long> SkuIds { get; set; }

        public long BenefitTypeId { get; set; }
        public long BenefitOrCategoryId { get; set; }
        public string BenefitType { get; set; }
        public string BenefitOrCategory { get; set; }

        public decimal SapDays { get; set; }
        public decimal NonSapDiscount { get; set; }

        public decimal DiscountOrDays { get; set; }
        public long PercentileNumber { get; set; }

        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public long LoginUserId { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }

        public List<SurpriseBenefitUserMappingDto> SurpriseBenefitUserMappingDto { get; set; }

        public SurpriseBenefitUserDto()
        {
            SurpriseBenefitUserMappingDto = new List<SurpriseBenefitUserMappingDto>();
        }
    }
}

