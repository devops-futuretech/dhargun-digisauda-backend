using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SkuDiscountUserDto : IAPIInputDTO
    {
        public long Id { get; set; }
        public long SkuDiscountHistoryId  { get; set; }
        public string Name { get; set; }
        public long VerticalId { get; set; }
        public List<long> OilTypeIds { get; set; }
        public List<long> OilPackingTypeIds { get; set; }  /* BPOrCPWise - PackGroup - OilPackingTypeId*/
        public List<long> CustomerGroupIds { get; set; }
        public List<long> CustomerIds { get; set; }
        public List<long> ZonalHeadIds { get; set; }
        public List<long> BDOIds { get; set; }
        public List<long> SkuIds { get; set; }

        public decimal Discount { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string Vertical { get; set; }

        public long LoginUserId { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public bool IsEdit { get; set; }
        public bool IsActive { get; set; }
        public List<SkuDiscountUserMappingDto> SkuDiscountUserDetailList { get; set; }
        public SkuDiscountUserDto()
        {
            SkuDiscountUserDetailList = new List<SkuDiscountUserMappingDto>();
        }
    }
}

