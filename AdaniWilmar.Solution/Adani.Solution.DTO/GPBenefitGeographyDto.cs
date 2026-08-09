using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class GPBenefitGeographyDto : IAPIInputDTO
    {
        public long Id { get; set; }
        public long GPBenefitHistoryId { get; set; }
        public long VerticalId { get; set; }
        public List<long> OilTypeIds { get; set; }
        //BPOrCPWise
        public List<long> OilPackingTypeIds { get; set; }
        public List<long> CustomerGroupIds { get; set; }
        public List<long> CustomerIds { get; set; }
        public List<long> SkuIds { get; set; }
        public List<int> CityIds { get; set; }

        public List<long> ZoneIds { get; set; }
        public List<int> StateIds { get; set; }
        public List<int> DistrictIds { get; set; }
        public List<int> TerritoryIds { get; set; }

        public decimal Days { get; set; }
        public decimal Discount { get; set; }

        public List<long> BenefitsIds { get; set; }

        public string BenefitType { get; set; }
        public long BenefitTypeId { get; set; }
        public string Benefit { get; set; }
        public long BenefitOrCategoryId { get; set; }

        public bool IsActive { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public long LoginUserId { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public bool IsEdit { get; set; }
        public string Vertical { get; set; }

        public List<GPBenefitGeographyMappingDto> GPBenefitGeographyMappingDtoList { get; set; }

        public GPBenefitGeographyDto()
        {
            GPBenefitGeographyMappingDtoList = new List<GPBenefitGeographyMappingDto>();
        }
    }
}
