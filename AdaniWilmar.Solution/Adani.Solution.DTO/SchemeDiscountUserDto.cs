using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SchemeDiscountUserDto : IAPIInputDTO
    {
        public long Id { get; set; }
        public long SchemeDiscountHistoryId { get; set; }
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


        public bool IsActive { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public long LoginUserId { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public bool IsEdit { get; set; }

        public string Vertical { get; set; }

        public List<SchemeDiscountUserMappingDto> SchemeDiscountUserDetailList { get; set; }
        public SchemeDiscountUserDto()
        {
            SchemeDiscountUserDetailList = new List<SchemeDiscountUserMappingDto>();
        }
    }
}

