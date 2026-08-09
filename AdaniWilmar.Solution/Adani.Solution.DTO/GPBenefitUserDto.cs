using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class GPBenefitUserDto : IAPIInputDTO
    {
        public long Id { get; set; }
        public long GPBenefitHistoryId { get; set; }
        public long VerticalId { get; set; }
        public List<long> OilTypeIds { get; set; }
        //BPOrCPWise
        public List<long> OilPackingTypeIds { get; set; }
        public List<long> CustomerGroupIds { get; set; }
        public List<long> CustomerIds { get; set; }
        public List<long> ZonalHeadIds { get; set; }
        public List<long> BDOIds { get; set; }
        public List<long> SkuIds { get; set; }

        public long BenefitTypeId { get; set; }
        public long BenefitOrCategoryId { get; set; }
        public string BenefitType { get; set; }
        public string BenefitOrCategory { get; set; }

        public decimal Days { get; set; }
        public decimal Discount { get; set; }

        public bool IsActive { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public bool IsEdit { get; set; }

        public long LoginUserId { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public string Vertical { get; set; }

        public List<GPBenefitUserMappingDto> GPBenefitUserMappingDtoList { get; set; }
        public GPBenefitUserDto()
        {
            GPBenefitUserMappingDtoList = new List<GPBenefitUserMappingDto>();
        }
    }
}

